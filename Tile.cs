using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Tile
{
    // Create constant for zipline tile type
    public const int ZIPLINE = 1000; // Zipline start and end tiles are from here and onwards in pairs

    // Create constants for if tile is floodable or not
    public const bool NOT_FLOODED = false;
    public const bool FLOODED = true;

    // Create constants for core functional tiles (NOTE: StartIndex has to be placed after starting tile, so string conversion chooses real tile)
    public enum Func
    {
        ButtonStart = -2, // Button priority helper value
        Null = -1,
        Flood = 0,
        StartIndex = Flood,
        Barrier,
        Empty,
        Water,
        Start,
        End,
        Button,
        PressedButton,
        WallJump,
        EndIndex,
    }

    // Create constants for platform tiles
    public enum Plat
    {
        Grass = 100,
        StartIndex = Grass,
        Snow,
        Sand,
        Dirt,
        Stone,
        Metal,
        Gold,
        Plank,
        Pike,
        Crate,
        Trunk,
        EndIndex
    }

    // Create constants for decoration tiles
    public enum Decor
    {
        Ladder = 500,
        StartIndex = Ladder,
        Leaf,
        AlertSign,
        LeftSign,
        RightSign,
        Fern,
        Cactus,
        Tree,
        EndIndex
    }

    // Create constant for color tiles
    public enum Clr
    {
        Black = 900,
        StartIndex = Black,
        Grey,
        White,
        Red,
        Orange,
        Yellow,
        Lime,
        Green,
        Teal,
        Cyan,
        Sky,
        Blue,
        Indigo,
        Purple,
        Magenta,
        Pink,
        EndIndex
    }

    // Store all tile textures
    public static Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();
    public static Texture2D SelectImg { get; set; }
    private static Texture2D ziplineImg;

    // Store tile information
    public Point Pos { get; private set; }
    public int Type { get; set; }
    public Rectangle Rec { get; }

    public Tile(int posX, int posY, int type = (int)Func.Empty)
    {
        Pos = new Point(posX, posY);
        Type = type;
        Rec = new Rectangle(posX * Game1.TILE_SIZE * Game1.PIXEL_SCALE, posY * Game1.TILE_SIZE * Game1.PIXEL_SCALE, Game1.TILE_SIZE * Game1.PIXEL_SCALE, Game1.TILE_SIZE * Game1.PIXEL_SCALE);
    }

    public static bool CanCollide(int type)
    {
        return (type >= (int)Func.StartIndex && type < (int)Func.EndIndex)
            || type == (int)Func.WallJump;
    }

    public static void LoadTextures(ContentManager content)
    {
        // Load all functional tiles
        for (Func type = Func.StartIndex; type < Func.EndIndex; type++)
        {
            textures[(int)type] = content.Load<Texture2D>($"Images/Sprites/Tiles/Functional/{type}");
        }

        // Load all platform tiles
        for (Plat type = Plat.StartIndex; type < Plat.EndIndex; type++)
        {
            textures[(int)type] = content.Load<Texture2D>($"Images/Sprites/Tiles/Platform/{type}");
        }

        // Load all decoration tiles
        for (Decor type = Decor.StartIndex; type < Decor.EndIndex; type++)
        {
            textures[(int)type] = content.Load<Texture2D>($"Images/Sprites/Tiles/Decoration/{type}");
        }

        // Load all color tiles
        for (Clr type = Clr.StartIndex; type < Clr.EndIndex; type++)
        {
            textures[(int)type] = content.Load<Texture2D>($"Images/Sprites/Tiles/Color/{type}");
        }

        // Load other editor tiles not included in tile types
        SelectImg = content.Load<Texture2D>("Images/Sprites/Tiles/Editor/SelectTile");
        ziplineImg = content.Load<Texture2D>("Images/Sprites/Tiles/Editor/ZiplineTile");
    }

    public static int GetType(int type, bool isEditing = false)
    {
        // Perform button logic if button or flood
        if (type <= (int)Func.ButtonStart)
        {
            if (isEditing || Game1.player == null) return (int)Func.Button;
            return (Game1.player.Buttons.Find(new Button(0, 0, Button.TypeToPriority(type))) != null) ? (int)Func.Button : (int)Func.PressedButton;
        }
        else if (type >= ZIPLINE)
        {
            return ZIPLINE;
        }
        else if (!isEditing && type == (int)Func.Flood)
        {
            return (int)Func.Water;
        }

        return type;
    }

    public Tile Copy()
    {
        return new Tile(Pos.X, Pos.Y, Type);
    }

    public void Draw(SpriteBatch spriteBatch, bool editing = false)
    {
        // Draw the tile if it can be drawn and should be visible
        int type = GetType(Type, editing);

        if ((type > (int)Func.Empty || type == (int)Func.Barrier || type == (int)Func.Flood)
            && (editing || (type != (int)Func.Start && type != (int)Func.End && type != (int)Func.Barrier && type != (int)Func.Flood))
            && (type < ZIPLINE))
        {
            spriteBatch.Draw(GetTexture(type), Rec, Color.White);
        }
    }

    public static void LoadEditorOnlyTiles(ContentManager content)
    {
        SelectImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/SelectTile");
        ziplineImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/ZiplineTile");
    }

    public static Texture2D GetTexture(int type)
    {
        if (type == ZIPLINE) return ziplineImg;
        return textures[type];
    }
}