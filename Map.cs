using System;
using System.IO;
using System.Timers;
using Microsoft.Xna.Framework.Graphics;

class Map // TODO All documentation for methods
{
    // Store core map information
    private string name;
    private float difficulty;
    private DateTime creationDate;
    private DateTime modifiedDate;
    private bool locked;

    // Store map behaviour information
    private int tileCnt = 0;
    private int startTileCnt = 0;
    
    private float floodSpeed = 0.1f; // Water spread speed (tiles per second)
    private Timer floodTimer; // Current timer for water spread

    // Store tiles in the map
    private Tile[,] tiles;
    private Tile[,] bgTiles;

    public Map() {}

    public Map(string name, float difficulty, int mapSize, bool locked = false)
    {
        // Initialize map information and create tile arrays
        this.name = name;
        this.difficulty = difficulty;
        creationDate = DateTime.Now;
        modifiedDate = DateTime.Now;
        this.locked = locked;

        tiles = new Tile[mapSize, mapSize];
        bgTiles = new Tile[mapSize, mapSize];

        // Setup all tiles (starting as empty)
        for (int r = 0; r < tiles.GetLength(0); r++)
        {
            for (int c = 0; c < tiles.GetLength(1); c++)
            {
                tiles[r, c] = new Tile();
                bgTiles[r, c] = new Tile();
            }
        }
    }

    public void Start()
    {
        // TODO idek
    }

    public void Update()
    {
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw each tile, background tile first, then foreground tile
        for (int r = 0; r < tiles.GetLength(0); r++)
        {
            for (int c = 0; c < tiles.GetLength(1); c++)
            {
                bgTiles[r, c].Draw(spriteBatch);
                tiles[r, c].Draw(spriteBatch);
            }
        }
    }

    public void Save(StreamWriter outFile)
    {
        
    }

    public void Load(StreamReader inFile)
    {
        // Create a variable for storing lines
        string [] line;

        // Load core map information
        name = inFile.ReadLine();
        difficulty = Convert.ToSingle(inFile.ReadLine());
        creationDate = Convert.ToDateTime(inFile.ReadLine());
        modifiedDate = Convert.ToDateTime(inFile.ReadLine());
        locked = Convert.ToBoolean(inFile.ReadLine());

        // Load map behaviour information
        tileCnt = Convert.ToInt32(inFile.ReadLine());
        startTileCnt = Convert.ToInt32(inFile.ReadLine());
        floodSpeed = Convert.ToSingle(inFile.ReadLine());

        // Load in map tiles
        for (int r = 0; r < tiles.GetLength(0); r++)
        {
            line = inFile.ReadLine().Split(' ');

            for (int c = 0; c < tiles.GetLength(1); c++)
            {
                tiles[r, c] = new Tile(Convert.ToInt32(line[c]));
            }
        }

        // Load in map tiles (bg)
        for (int r = 0; r < tiles.GetLength(0); r++)
        {
            line = inFile.ReadLine().Split(' ');

            for (int c = 0; c < tiles.GetLength(1); c++)
            {
                bgTiles[r, c] = new Tile(Convert.ToInt32(line[c]));
            }
        }
    }
}