using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Map
{
    // Create constants for text file names
    public const string SAVE_FILE = "SavedMaps.txt";
    public const string DEFAULT_SAVE_FILE = "DefaultSavedMaps.txt";

    // Create constant for required amount of default maps
    public const int DEFAULT_MAPS = 2; // TODO change after adding all maps

    // Create constant for empty variables
    public const int EMPTY = -1;
    public const string MISSING_BEST_TIME = "No Best Time";

    // Create new map constants
    public const string DEFAULT_DESC = "A newly created map template.";
    public const int MAX_SIZE = 1000;

    // Create read only array for map difficulties
    public const int MIN_DIFF = 0;
    public const int MAX_EXC_DIFF = 8;

    public static readonly Color[] diffColors = {
        Color.White,        // 0 - Other (N/A)
        Color.Green,        // 1 - Easy
        Color.Yellow,       // 2 - Medium
        Color.DarkRed,      // 3 - Hard
        Color.Purple,       // 4 - Intense
        Color.DarkOrange,   // 5 - Crazy
        Color.OrangeRed,    // 6 - Merciless
        Color.Blue,         // 7 - Extreme
    };

    // Store adjacent tile transformations
    private readonly (int, int)[] adjTiles = {(-1, 0), (1, 0), (0, -1), (0, 1)};

    // Store core map information
    public string Name { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string Song { get; set; }
    public float Difficulty { get; set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }
    public bool Locked {get; private set; }

    // Sort a list of maps by a given property
    public static void Sort(List<Map> maps, Comparison<Map> comparison) => MergeSort.Sort(maps, comparison);

    // Create the comparisons for the allowed options (properties) to sort by // REVIEW do i need to document all of these comparators as functions cuz they are just fields technically?
    public static readonly Comparison<Map> SortByName         = (a, b) => a.Name.CompareTo(b.Name);
    public static readonly Comparison<Map> SortByDifficulty   = (a, b) => a.Difficulty.CompareTo(b.Difficulty);
    public static readonly Comparison<Map> SortByCreationDate = (a, b) => a.CreationDate.CompareTo(b.CreationDate);
    public static readonly Comparison<Map> SortByModifiedDate = (a, b) => a.ModifiedDate.CompareTo(b.ModifiedDate);
    public static readonly Comparison<Map> SortByBestTime     = (a, b) => {
        // If best time is empty, sort it last (put it at the end)
        if (a.BestTime == b.BestTime) return 0;
        if (a.BestTime == EMPTY) return 1;
        if (b.BestTime == EMPTY) return -1;

        // Otherwise sort by fastest time in non-descending order
        return a.BestTime.CompareTo(b.BestTime);
    };

    public float Time { get; private set; } = 0; // seconds
    public float BestTime { get; set; } = EMPTY; // seconds

    // Store map behaviour information
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    private Vector2 startPos;

    public float DrownSpeed { get; set; } = 10f;    // Oxygen depletion amount when in water
    public float FloodSpeed { get; set; } = 0.1f;   // Water spread speed (tiles per second)

    // Store tiles in the map
    public Tile[,] Tiles { get; set; }
    public Tile[,] BgTiles { get; set; }
    public int[,] FloodTiles { get; set; }

    // Store ziplines in the map
    private int ziplineCnt = 0;
    public List<Zipline> Ziplines { get; set; }

    // Store buttons in the map
    public BSTree<Button> Buttons { get; set; } = new BSTree<Button>();

    // Store buttons in the map
    private Queue<(int, int)> floodQueue;
    private Timer floodTimer; // Current timer for water spread

    public Map() {}

    public Map(string name, string author, float difficulty = 0.0f, int sizeX = 10, int sizeY = 10, bool locked = false)
    {
        // Initialize map information
        Name = name;
        Author = author;
        Difficulty = difficulty;
        Song = SoundManager.NO_SONG;
        Locked = locked;
        SizeX = sizeX;
        SizeY = sizeY;

        // Update map dates
        CreationDate = DateTime.Now;
        ModifiedDate = DateTime.Now;

        // Create template description
        Description = DEFAULT_DESC + $" Size of {sizeX}x{sizeY}.";

        // Try to default map
        try
        {
            // Load the file into this map
            StreamReader inFile = new StreamReader("DefaultMap.txt");
            Load(inFile, true);
        }
        catch
        {
            // Print what failed
            Console.WriteLine("ERROR - New map template failed to load.");
        }
    }

    public void Start(Player player)
    {
        // Reset player data
        player.ResetPlayer();

        // Set starting pause state to unpaused
        Game1.paused = false;

        // Reset timer
        Time = 0;

        // Give player a copy of all the buttons (for the player to go through like a priority queue)
        player.NextButton = Buttons.GetLeftmost();
        player.Buttons = Buttons.Copy();

        // Reset flood queue and timer
        floodQueue = new Queue<(int, int)>();
        floodTimer = new Timer(1000 / FloodSpeed, true);

        // Create new flood tiles
        FloodTiles = new int[SizeX, SizeY];

        // Reset all water tiles to unflooded, also add flood start tiles into queue
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                // Find start pos
                if (Tiles[x, y].Type == (int)Tile.Func.Start)
                {
                    startPos = new Vector2(x + 0.5f, y + 0.5f) * (Game1.TILE_SIZE * Game1.PIXEL_SCALE);
                }

                // Reset flood, and if the tile is a flood start tile, put it into the queue
                if (Tile.IsFlood(Tiles[x, y].Type))
                {
                    FloodTiles[x, y] = Tiles[x, y].Type;
                    floodQueue.Enqueue((x, y));
                }
                else
                {
                    FloodTiles[x, y] = Tile.NOT_FLOODED;
                }
            }
        }

        // Center player and camera
        player.CenterPos(startPos);
        player.Camera.SetPos(startPos);
        player.Camera.ResetZoom();
    }

    public void Update(GameTime gameTime)
    {
        // Store current tile and adj tile coordinates
        int x;
        int y;
        int nx;
        int ny;

        // Update elapsed time
        Time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update flood timer
        floodTimer.Update(gameTime.ElapsedGameTime.Milliseconds);

        // Propagate flood if flood timer is finished
        if (floodTimer.IsFinished())
        {
            // Only loop amount of tiles in current depth
            for (int amount = floodQueue.Count; amount-- > 0;)
            {
                // Get current tile
                (x, y) = floodQueue.Dequeue();

                // Explore adjacent tiles
                foreach ((int xi, int yi) in adjTiles)
                {
                    // Calculate new tiles
                    nx = x + xi;
                    ny = y + yi;

                    // Only perform logic if tile within bounds
                    if (nx < 0 || nx >= SizeX || ny < 0 || ny >= SizeY) continue;

                    // Store tile type
                    int type = Tile.GetType(Tiles[nx, ny].Type);

                    // Only expand tile if empty (and unflooded)
                    if (FloodTiles[nx, ny] == Tile.NOT_FLOODED && !Tile.CanCollide(type))
                    {
                        // Set the tile to flooded and add it to the queue
                        FloodTiles[nx, ny] = FloodTiles[x, y];
                        floodQueue.Enqueue((nx, ny));
                    }
                }
            }

            // Reset flood timer
            floodTimer.ResetTimer(true);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw each tile, background tile first, 
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                // Draw background tile first
                BgTiles[x, y].Draw(spriteBatch);

                // Draw expanding water while keeping background and foreground in mind
                if (Tile.IsFlood(FloodTiles[x, y]) && Tiles[x, y].Type != (int)Tile.Func.WallJump) spriteBatch.Draw(Tile.textures[Tile.GetType(FloodTiles[x, y])], Tiles[x, y].Rec, Color.White);

                // Draw foreground tile last
                Tiles[x, y].Draw(spriteBatch);

                // If water floods a ladder, should go above it
                if (Tile.IsFlood(FloodTiles[x, y]) && Tiles[x, y].Type == (int)Tile.Decor.Ladder) spriteBatch.Draw(Tile.textures[Tile.GetType(FloodTiles[x, y])], Tiles[x, y].Rec, Color.White);
            }
        }

        // Draw all the ziplines if we are drawing the foreground
        foreach (Zipline zipline in Ziplines)
        {
            zipline.Draw(spriteBatch);
        }
    }

    public void Save(StreamWriter outFile)
    {
        // Write core map information
        outFile.WriteLine(Name);
        outFile.WriteLine(Author);
        outFile.WriteLine(Description);
        outFile.WriteLine(Song);

        outFile.WriteLine(Difficulty);
        outFile.WriteLine(CreationDate);
        outFile.WriteLine(ModifiedDate);
        outFile.WriteLine(Locked);
        outFile.WriteLine(BestTime);

        outFile.WriteLine(SizeX);
        outFile.WriteLine(SizeY);

        // Write map behaviour information
        outFile.WriteLine(Ziplines.Count);
        outFile.WriteLine(DrownSpeed);
        outFile.WriteLine(FloodSpeed);

        // Write foreground tiles
        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                outFile.Write(Tiles[x, y].Type + (x == SizeX - 1 ? "\n" : " "));
            }
        }

        // Write background tiles
        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                outFile.Write(BgTiles[x, y].Type + (x == SizeX - 1 ? "\n" : " "));
            }
        }
    }

    public void Load(StreamReader inFile, bool newMap = false)
    {
        // Create a variable for storing lines
        string[] line;

        // Load core map information only if this is not a new map
        if (!newMap)
        {
            Name = inFile.ReadLine();
            Author = inFile.ReadLine();
            Description = inFile.ReadLine();
            Song = inFile.ReadLine();

            Difficulty = Convert.ToSingle(inFile.ReadLine());
            CreationDate = Convert.ToDateTime(inFile.ReadLine());
            ModifiedDate = Convert.ToDateTime(inFile.ReadLine());
            Locked = Convert.ToBoolean(inFile.ReadLine());
            BestTime = Convert.ToSingle(inFile.ReadLine());

            SizeX = Convert.ToInt32(inFile.ReadLine());
            SizeY = Convert.ToInt32(inFile.ReadLine());
        }

        // Load map behaviour information
        ziplineCnt = Convert.ToInt32(inFile.ReadLine());
        DrownSpeed = Convert.ToSingle(inFile.ReadLine());
        FloodSpeed = Convert.ToSingle(inFile.ReadLine());

        // Create tile arrays
        Tiles = new Tile[SizeX, SizeY];
        BgTiles = new Tile[SizeX, SizeY];
        FloodTiles = new int[SizeX, SizeY];

        // Create the list to store ziplines
        Ziplines = new Zipline[ziplineCnt].ToList();

        // Load in map tiles
        for (int y = 0; y < SizeY; y++)
        {
            line = inFile.ReadLine().Split(' ');

            for (int x = 0; x < SizeX; x++)
            {
                Tiles[x, y] = new Tile(x, y, Convert.ToInt32(line[x]));

                // Keep track of special tiles
                if (Tiles[x, y].Type >= Tile.ZIPLINE)
                {
                    // Create variables to easily access zipline indexes and properties
                    int id = Zipline.GetId(Tiles[x, y].Type);

                    // Create a new zipline if there is none
                    if (Ziplines[id] == null)
                    {
                        Ziplines[id] = new Zipline();
                    }

                    // Set the zipline's start and end tile, depending on if this tile is the start or end
                    if (Zipline.IsStart(Tiles[x, y].Type))
                    {
                        Ziplines[id].Start = Tiles[x, y];
                    }
                    else
                    {
                        Ziplines[id].End = Tiles[x, y];
                    }
                }
                else if (Tiles[x, y].Type <= (int)Tile.Func.ButtonStart)
                {
                    // Add button into BST
                    Buttons.Add(new Button(x, y, Button.TypeToPriority(Tiles[x, y].Type)));
                }
            }
        }

        // Load in map tiles (bg)
        for (int y = 0; y < SizeY; y++)
        {
            line = inFile.ReadLine().Split(' ');

            for (int x = 0; x < SizeX; x++)
            {
                BgTiles[x, y] = new Tile(x, y, Convert.ToInt32(line[x]));
            }
        }

        // Load all zipline shapes
        foreach (Zipline zipline in Ziplines) zipline.Load();
    }

    public void UpdateModifiedDate()
    {
        ModifiedDate = DateTime.Now;
    }
}