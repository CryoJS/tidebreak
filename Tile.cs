using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tidebreak;

class Tile
{
    // Create constants for tile types
    public const int TEXTURE_TYPE_AMOUNT = 25;
    public const int PLATFORM_TYPE_AMOUNT = 12; // includes EMPTY
    public const int DECORATIVE_TYPE_AMOUNT = 7;
    public const int FUNCTIONAL_TYPE_AMOUNT = 8;

    public const int BUTTON_START = -5;
    public const int NULL = -4;
    public const int FLOOD = -3;
    public const int BARRIER = -2;
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
    public const int TRUNK = 11;

    public const int LADDER = 12;
    public const int LEAF = 13;
    public const int SIGN = 14;
    public const int LEFT_SIGN = 15;
    public const int RIGHT_SIGN = 16;
    public const int FERN = 17;
    public const int CACTUS = 18;
    public const int TREE = 19;

    // Create constants for special tile types
    public const int START = 20;
    public const int END = 21;
    public const int BUTTON = 22; // Only for visuals, buttons are stored as #s < NULL
    public const int PRESSED_BUTTON = 23;
    public const int WALL_JUMP = 24;
    public const int ZIPLINE = 50; // Zipline start and end tiles are from here and onwards in pairs, i.e. {(50, 51), (52, 53), ...}

    // Create constants for if tile is floodable or not
    public const int NOT_FLOODED = 0;
    public const int FLOOD_START = 1;
    public const int FLOODED = 2;

    // Store all tile textures
    public static Texture2D[] textures = new Texture2D[TEXTURE_TYPE_AMOUNT];
    public static Texture2D SelectImg { get; set; }
    private static Texture2D barrierImg;
    private static Texture2D emptyImg;
    private static Texture2D ziplineImg;
    private static Texture2D floodImg;

    // Store tile information
    private Point pos;
    public int Type { get; set; }
    public Rectangle Rec { get; }

    public Tile(int posX, int posY, int type = EMPTY)
    {
        pos = new Point(posX, posY);
        Type = type;
        Rec = new Rectangle(posX * Game1.TILE_SIZE * Game1.PIXEL_SCALE, posY * Game1.TILE_SIZE * Game1.PIXEL_SCALE, Game1.TILE_SIZE * Game1.PIXEL_SCALE, Game1.TILE_SIZE * Game1.PIXEL_SCALE);
    }

    public static int GetType(int type, bool isEditing = false)
    {
        // Perform button logic if button
        if (type <= BUTTON_START)
        {
            if (isEditing || Game1.player == null) return BUTTON;
            return (Game1.player.Buttons.Find(new Button(0, 0, Button.TypeToPriority(type))) != null) ? BUTTON : PRESSED_BUTTON;
        }

        return type;
    }

    public Tile Copy()
    {
        return new Tile(pos.X, pos.Y, Type);
    }

    public void Draw(SpriteBatch spriteBatch, bool editing = false)
    {
        // Draw the tile if it can be drawn and should be visible
        int type = GetType(Type, editing);

        if ((type > EMPTY || type == BARRIER || type == FLOOD) && (editing || (type != START && type != END && type != BARRIER && type != FLOOD)) && (type < ZIPLINE))
        {
            spriteBatch.Draw(GetTexture(type), Rec, Color.White);
        }
    }

    public static void LoadEditorOnlyTiles(ContentManager content)
    {
        SelectImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/SelectTile");
        barrierImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/BarrierTile");
        emptyImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/EmptyTile");
        ziplineImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/ZiplineTile");
        floodImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/FloodTile");
    }

    public static Texture2D GetTexture(int type)
    {
        if (type == FLOOD) return floodImg;
        else if (type == BARRIER) return barrierImg;
        else if (type == EMPTY) return emptyImg;
        else if (type == ZIPLINE) return ziplineImg;
        return textures[type];
    }
}