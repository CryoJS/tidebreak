using System;
using System.Collections.Generic;
using System.IO;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Map // TODO All documentation for methods
{
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
        Color.White,        // 0 - Simple
        Color.Green,        // 1 - Easy
        Color.Yellow,       // 2 - Medium
        Color.DarkRed,      // 3 - Hard
        Color.Purple,       // 4 - Intense
        Color.DarkOrange,   // 5 - Crazy
        Color.OrangeRed,    // 6 - Merciless
        Color.Blue,         // 7 - Extreme
    };

    // Store adjacent tile transformations
    (int, int)[] adjTiles = {(-1, 0), (1, 0), (0, -1), (0, 1)};

    // Store core map information
    public string name { get; set; }
    public string author { get; set; }
    public string description { get; set; }
    public float difficulty { get; set; }
    public DateTime creationDate { get; private set; }
    public DateTime modifiedDate { get; private set; }
    public bool locked {get; private set; }

    public float time { get; private set; } = 0; // seconds
    public float bestTime { get; set; } = EMPTY; // seconds

    // Store map behaviour information
    public int sizeX { get; set; }
    public int sizeY { get; set; }
    private int startTileCnt = 0;
    private Vector2 startPos;

    public float drownSpeed { get; set; } = 10f;    // Oxygen depletion amount when in water
    public float floodSpeed { get; set; } = 0.1f;   // Water spread speed (tiles per second)

    // Store tiles in the map
    public Tile[,] tiles { get; set; }
    public Tile[,] bgTiles { get; set; }
    public int[,] floodTiles { get; set; }

    // Store ziplines in the map
    private int ziplineCnt = 0;
    public List<Zipline> ziplines { get; private set; }

    // Store buttons in the map
    int buttonCnt;
    public BSTree<Button> buttons { get; private set; } = new BSTree<Button>();

    // Store buttons in the map
    private Queue<(int, int)> floodQueue;
    private Timer floodTimer; // Current timer for water spread

    public Map() {}

    // TODO: use load map to update these parameters
    public Map(string name, string author, float difficulty = 0.0f, int sizeX = 50, int sizeY = 15, bool locked = false)
    {
        // Initialize map information
        this.name = name;
        this.author = author;
        this.difficulty = difficulty;
        this.locked = locked;
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        // Update map dates
        creationDate = DateTime.Now;
        modifiedDate = DateTime.Now;

        // Create template description
        description = DEFAULT_DESC + $" Size of {sizeX}x{sizeY}.";

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

    public void Start(Player player, Camera camera)
    {
        // Center player and camera
        player.CenterPos(startPos);
        camera.SetPos(startPos);

        // Reset player data
        player.ResetPlayer();

        // Set starting pause state to unpaused
        Game1.paused = false;

        // Reset timer
        time = 0;

        // Give player a copy of all the buttons (for the player to go through like a priority queue)
        player.NextButton = buttons.GetLeftmost();
        player.Buttons = buttons.Copy();

        // Reset flood queue and timer
        floodQueue = new Queue<(int, int)>();
        floodTimer = new Timer(1000 / floodSpeed, true);

        // Reset all buttons to unpressed and water tiles to unflooded, also add flood start tiles into queue
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                // Reset buttons
                if (tiles[x, y].Type == Tile.PRESSED_BUTTON)
                {
                    tiles[x, y].Type = Tile.BUTTON;
                }

                // Reset flood, and if the tile is a flood start tile, put it into the queue
                if (floodTiles[x, y] == Tile.FLOODED)
                {
                    floodTiles[x, y] = Tile.NOT_FLOODED;
                }
                else if (floodTiles[x, y] == Tile.FLOOD_START)
                {
                    floodQueue.Enqueue((x, y));
                }
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        // Store current tile and adj tile coordinates
        int x;
        int y;
        int nx;
        int ny;

        // Update elapsed time
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                    if (nx < 0 || nx > sizeX || ny < 0 || ny > sizeY) continue;

                    // Only expand tile if empty (and unflooded)
                    if (floodTiles[nx, ny] == Tile.NOT_FLOODED && tiles[nx, ny].Type != Tile.WALL_JUMP && (tiles[nx, ny].Type == Tile.EMPTY || tiles[nx, ny].Type > Tile.PLATFORM_TYPE_AMOUNT))
                    {
                        // Set the tile to flooded and add it to the queue
                        floodTiles[nx, ny] = Tile.FLOODED;
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
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                // Draw background tile first
                bgTiles[x, y].Draw(spriteBatch);

                // Draw expanding water while keeping background and foreground in mind
                if (floodTiles[x, y] != Tile.NOT_FLOODED && floodTiles[x, y] != Tile.WALL_JUMP)
                {
                    spriteBatch.Draw(Tile.tileTextures[Tile.WATER], tiles[x, y].Rec, Color.White);
                }

                // Draw foreground tile last
                tiles[x, y].Draw(spriteBatch);
            }
        }

        // Draw all the ziplines
        foreach (Zipline zipline in ziplines)
        {
            zipline.Draw(spriteBatch);
        }
    }

    public void Save(StreamWriter outFile)
    {
        // TODO
    }

    public void Load(StreamReader inFile, bool newMap = false)
    {
        // Create a variable for storing lines
        string[] line;

        // Load core map information only if this is not a new map
        if (!newMap)
        {
            name = inFile.ReadLine();
            author = inFile.ReadLine();
            description = inFile.ReadLine();
            difficulty = Convert.ToSingle(inFile.ReadLine());
            creationDate = Convert.ToDateTime(inFile.ReadLine());
            modifiedDate = Convert.ToDateTime(inFile.ReadLine());
            locked = Convert.ToBoolean(inFile.ReadLine());

            sizeX = Convert.ToInt32(inFile.ReadLine());
            sizeY = Convert.ToInt32(inFile.ReadLine());
        }

        // Load map behaviour information
        startTileCnt = Convert.ToInt32(inFile.ReadLine());
        ziplineCnt = Convert.ToInt32(inFile.ReadLine());
        drownSpeed = Convert.ToSingle(inFile.ReadLine());
        floodSpeed = Convert.ToSingle(inFile.ReadLine());

        // Create tile arrays
        tiles = new Tile[sizeX, sizeY];
        bgTiles = new Tile[sizeX, sizeY];
        floodTiles = new int[sizeX, sizeY];

        // Create the list to store ziplines
        ziplines = new List<Zipline>(new Zipline[ziplineCnt]);

        // Load in map tiles
        for (int y = 0; y < sizeY; y++)
        {
            line = inFile.ReadLine().Split(' ');

            for (int x = 0; x < sizeX; x++)
            {
                tiles[x, y] = new Tile(x, y, Convert.ToInt32(line[x]));

                // Keep track of special tiles
                if (tiles[x, y].Type == Tile.START)
                {
                    startPos = new Vector2(x * Game1.TILE_SIZE * Game1.PIXEL_SCALE, y * Game1.TILE_SIZE * Game1.PIXEL_SCALE);
                }
                else if (tiles[x, y].Type >= Tile.ZIPLINE)
                {
                    // Create variables to easily access zipline indexes and properties
                    int id = Zipline.FindId(tiles[x, y].Type);

                    // Create a new zipline if there is none
                    if (ziplines[id] == null)
                    {
                        ziplines[id] = new Zipline();
                    }

                    // Set the zipline's start and end tile, depending on if this tile is the start or end
                    if (Zipline.IsStart(tiles[x, y].Type))
                    {
                        ziplines[id].Start = tiles[x, y];
                    }
                    else
                    {
                        ziplines[id].End = tiles[x, y];
                    }
                }
            }
        }

        // Load in map tiles (bg)
        for (int y = 0; y < sizeY; y++)
        {
            line = inFile.ReadLine().Split(' ');

            for (int x = 0; x < sizeX; x++)
            {
                bgTiles[x, y] = new Tile(x, y, Convert.ToInt32(line[x]));
            }
        }

        // Load in map tiles (flood tiles)
        for (int y = 0; y < sizeY; y++)
        {
            line = inFile.ReadLine().Split(' ');

            for (int x = 0; x < sizeX; x++)
            {
                floodTiles[x, y] = Convert.ToInt32(line[x]);
            }
        }

        // Load all zipline shapes
        foreach (Zipline zipline in ziplines)
        {
            zipline.Load(Game1._graphics.GraphicsDevice);
        }

        // Load in the # of buttons
        buttonCnt = Convert.ToInt32(inFile.ReadLine());

        // Load in all buttons into BST
        for (int i = 0; i < buttonCnt; i++)
        {
            // Store button line info
            line = inFile.ReadLine().Split(' ');
            int x = Convert.ToInt32(line[0]);
            int y = Convert.ToInt32(line[1]);

            // Add button into BST
            buttons.Add(new Button(x, y, Convert.ToInt32(line[2]), tiles[x, y].Rec.Center.ToVector2()));

            // As a safety, ensure buttons are drawn
            tiles[x, y].Type = Tile.BUTTON;
        }
    }
}