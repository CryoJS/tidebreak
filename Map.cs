using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Map // TODO All documentation for methods
{
    // Create constant for empty variables
    public const int EMPTY = -1;
    public const string MISSING_BEST_TIME = "No Best Time";

    // Create read only array for map difficulty colors
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

    // Store core map information
    public string name { get; private set; }
    public string author { get; private set; }
    public string description { get; private set; }
    public float difficulty { get; private set; }
    public DateTime creationDate { get; private set; } // FIXME might not add these yet
    public DateTime modifiedDate { get; private set; }
    public bool locked {get; private set; }

    public float time { get; private set; } = 0; // seconds
    public float bestTime { get; set; } = EMPTY; // seconds

    // Store map behaviour information
    public int sizeX { get; private set; }
    public int sizeY { get; private set; }
    private int startTileCnt = 0;
    private Vector2 startPos;

    public float oxygenSpeed { get; private set; } = 10f; // Oxygen depletion amount when in water
    private float floodSpeed = 0.1f; // Water spread speed (tiles per second)
    private GameUtility.Timer floodTimer; // Current timer for water spread // FIXME not sure if GameUtility.Timer or System.Timer(or something)

    // Store tiles in the map
    public Tile[,] tiles { get; private set; }
    private Tile[,] bgTiles;

    // Store ziplines in the map
    private int ziplineCnt = 0;
    public List<Zipline> ziplines { get; private set; }

    // Store buttons in the map
    int buttonCnt;
    public BSTree<Button> buttons { get; private set; } = new BSTree<Button>();

    public Map() {}

    // TODO: use load map to update these parameters
    public Map(string name, string author, float difficulty, int sizeX, int sizeY, bool locked = false)
    {
        // Initialize map information
        this.name = name;
        this.author = author;
        this.difficulty = difficulty;
        creationDate = DateTime.Now;
        modifiedDate = DateTime.Now;
        this.locked = locked;

        // Create tile arrays
        tiles = new Tile[sizeX, sizeY];
        bgTiles = new Tile[sizeX, sizeY];

        // Setup all tiles (starting as empty)
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y] = new Tile(x, y);
                bgTiles[x, y] = new Tile(x, y);
            }
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
        player.nextButton = buttons.GetLeftmost();
        player.buttons = buttons.Copy();

        // Reset all buttons to unpressed
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (tiles[x, y].type == Tile.PRESSED_BUTTON)
                {
                    tiles[x, y].type = Tile.BUTTON;
                }
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw each tile, background tile first, then foreground tile
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                bgTiles[x, y].Draw(spriteBatch);
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

    public void Load(StreamReader inFile, GraphicsDevice gd)
    {
        // Create a variable for storing lines
        string[] line;

        // Load core map information
        name = inFile.ReadLine();
        author = inFile.ReadLine();
        description = inFile.ReadLine();
        difficulty = Convert.ToSingle(inFile.ReadLine());
        creationDate = Convert.ToDateTime(inFile.ReadLine());
        modifiedDate = Convert.ToDateTime(inFile.ReadLine());
        locked = Convert.ToBoolean(inFile.ReadLine());

        // Load map behaviour information
        sizeX = Convert.ToInt32(inFile.ReadLine());
        sizeY = Convert.ToInt32(inFile.ReadLine());
        startTileCnt = Convert.ToInt32(inFile.ReadLine());
        ziplineCnt = Convert.ToInt32(inFile.ReadLine());
        oxygenSpeed = Convert.ToSingle(inFile.ReadLine());
        floodSpeed = Convert.ToSingle(inFile.ReadLine());

        // Create tile arrays
        tiles = new Tile[sizeX, sizeY];
        bgTiles = new Tile[sizeX, sizeY];

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
                if (tiles[x, y].type == Tile.START)
                {
                    startPos = new Vector2(x * Game1.TILE_SIZE * Game1.PIXEL_SCALE, y * Game1.TILE_SIZE * Game1.PIXEL_SCALE);
                }
                else if (tiles[x, y].type >= Tile.ZIPLINE)
                {
                    // Create variables to easily access zipline indexes and properties
                    int id = Zipline.FindId(tiles[x, y].type);

                    // Create a new zipline if there is none
                    if (ziplines[id] == null)
                    {
                        ziplines[id] = new Zipline();
                    }

                    // Set the zipline's start and end tile, depending on if this tile is the start or end
                    if (Zipline.IsStart(tiles[x, y].type))
                    {
                        ziplines[id].start = tiles[x, y];
                    }
                    else
                    {
                        ziplines[id].end = tiles[x, y];
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

        // Load all zipline shapes
        foreach (Zipline zipline in ziplines)
        {
            zipline.Load(gd);
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
            buttons.Add(new Button(x, y, Convert.ToInt32(line[2]), tiles[x, y].rec.Center.ToVector2()));

            // As a safety, ensure buttons are drawn
            tiles[x, y].type = Tile.BUTTON;
        }
    }
}