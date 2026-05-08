using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Timers;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Map // TODO All documentation for methods
{
    // Store core map information
    private string name;
    private float difficulty;
    private DateTime creationDate;
    private DateTime modifiedDate;
    private bool locked;

    // Store map behaviour information
    public int sizeX { get; private set; }
    public int sizeY { get; private set; }
    private int startTileCnt = 0;
    private Vector2 startPos;

    private float floodSpeed = 0.1f; // Water spread speed (tiles per second)
    private Timer floodTimer; // Current timer for water spread

    // Store tiles in the map
    public Tile[,] tiles { get; private set; }
    private Tile[,] bgTiles;

    // Store ziplines in the map
    int ziplineCnt = 0;
    List<Zipline> ziplines;

    public Map() { }

    public Map(string name, float difficulty, int sizeX, int sizeY, bool locked = false)
    {
        // Initialize map information
        this.name = name;
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

    public void Start(Player player)
    {
        player.CenterPos(startPos);
    }

    public void Update()
    {
        // TODO
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
        difficulty = Convert.ToSingle(inFile.ReadLine());
        creationDate = Convert.ToDateTime(inFile.ReadLine());
        modifiedDate = Convert.ToDateTime(inFile.ReadLine());
        locked = Convert.ToBoolean(inFile.ReadLine());

        // Load map behaviour information
        sizeX = Convert.ToInt32(inFile.ReadLine());
        sizeY = Convert.ToInt32(inFile.ReadLine());
        startTileCnt = Convert.ToInt32(inFile.ReadLine());
        ziplineCnt = Convert.ToInt32(inFile.ReadLine());
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
                    int id = tiles[x, y].type - Tile.ZIPLINE;
                    bool isStart = (id & 1) == (Tile.ZIPLINE & 1);

                    // Compress start and end pairs into one zipline
                    id /= 2;

                    // Create a new zipline if there is none
                    if (ziplines[id] == null)
                    {
                        ziplines[id] = new Zipline(gd);
                    }
                    
                    // Set the zipline's start and end tile, depending on if this tile is the start or end
                    if (isStart)
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
    }
}