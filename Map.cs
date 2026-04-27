using System;
using System.Timers;

class Map // TODO All documentation for methods
{
    // Store core map information
    private string name;
    private float difficulty;
    private DateTime creationDate;
    private DateTime modifiedDate;

    // Store map behaviour information
    private int tileCnt = 0;
    private int startTileCnt = 0;
    
    private float floodSpeed = 0.1f; // Water spread speed (tiles per second)
    private Timer floodTimer; // Current timer for water spread

    bool locked;

    // Store tiles in the map
    private Tile[,] tiles;

    public Map(string name, float difficulty, int mapSize, bool locked = false)
    {
        // Initialize map information and create tile array
        this.name = name;
        this.difficulty = difficulty;
        creationDate = DateTime.Now;
        modifiedDate = DateTime.Now;
        this.locked = locked;

        tiles = new Tile[mapSize, mapSize];

        // Setup all tiles (starting as empty)
        for (int r = 0; r < tiles.GetLength(0); r++)
        {
            for (int c = 0; c < tiles.GetLength(1); c++)
            {
                tiles[r, c] = new Tile();
            }
        }
    }
}