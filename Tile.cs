using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Tile
{
    // Create constants for tile types
    public const int TYPE_AMOUNT = 25;
    public const int PLATFORM_TYPE_AMOUNT = 13;

    public const int EMPTY = -1;
    public const int WATER = 0;
    public const int GRASS = 1;
    public const int SNOW = 2;
    public const int SAND = 3;
    public const int DIRT = 4;
    public const int STONE = 5;
    public const int METAL = 6;
    public const int GOLD = 7;
    public const int PLANK = 8;
    public const int PIKE = 9;
    public const int CRATE = 10;
    public const int LEAF = 11;
    public const int TRUNK = 12;
    public const int LADDER = 13;

    public const int SIGN = 14;
    public const int LEFT_SIGN = 15;
    public const int RIGHT_SIGN = 16;
    public const int FERN = 17;
    public const int CACTUS = 18;
    public const int TREE = 19;

    // Create constants for special tile types
    public const int START = 20;
    public const int END = 21;
    public const int BUTTON = 22;
    public const int PRESSED_BUTTON = 23;
    public const int WALL_JUMP = 24;
    public const int ZIPLINE = 50; // Zipline start and end tiles are from here and onwards in pairs, i.e. {(50, 51), (52, 53), ...}

    // Store all tile textures
    public static Texture2D[] tileTextures = new Texture2D[TYPE_AMOUNT];

    // Store tile information
    public int type { get; set; }
    public Rectangle rec { get; }

    public Tile(int posX, int posY, int type = EMPTY)
    {
        this.type = type;
        rec = new Rectangle(posX * Game1.TILE_SIZE * Game1.PIXEL_SCALE, posY * Game1.TILE_SIZE * Game1.PIXEL_SCALE, Game1.TILE_SIZE * Game1.PIXEL_SCALE, Game1.TILE_SIZE * Game1.PIXEL_SCALE);
    }

    public void Draw(SpriteBatch _spriteBatch, bool editing = false)
    {
        // Draw the tile if it can be drawn and should be visible
        if (type != EMPTY && (editing || (type != START && type != END)))
        {
            _spriteBatch.Draw(tileTextures[type], rec, Color.White);
        }
    }
}