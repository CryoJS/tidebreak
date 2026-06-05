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
    public const int FUNC_START = (int)Func.Flood;

    public enum Func
    {
        ButtonStart = -2, // Button priority helper value
        Null = -1,
        Flood = 0,
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
    public const int PLAT_START = (int)Plat.Grass;
    
    public enum Plat
    {
        Grass = 100,
        Snow,
        DryGrass,
        Dirt,
        Metal,
        Gold,
        Plank,
        Pike,
        Crate,
        Trunk,
        Stone,
        SmoothStone,
        StonePike,
        StoneBrick,
        SmallStoneBrick,
        Sand,
        SmoothSand,
        SandPike,
        SandBrick,
        SmallSandBrick,
        Marble,
        SmoothMarble,
        MarblePike,
        MarbleBrick,
        SmallMarbleBrick,
        Concrete,
        Box1,
        Box2,
        Beam1,
        Beam2,
        Beam3,
        Conveyor1,
        Conveyor2,
        Conveyor3,
        Panel1,
        Panel2,
        Panel3,
        Panel4,
        PipeX,
        PipeX2,
        PipeY,
        PipeY2,
        PipeUp,
        PipeDown,
        PipeLeft,
        PipeRight,
        PipeTL,
        PipeTR,
        PipeBL,
        PipeBR,
        SpillPipe1,
        SpillPipe2,
        Truss1,
        Truss2,
        Truss3,
        SupportTL,
        SupportTR,
        SupportBL,
        SupportBLW,
        SupportBR,
        SupportBRW,
        Barrel,
        EndIndex
    }

    // Create constants for decoration tiles
    public const int DECOR_START = (int)Decor.Ladder;

    public enum Decor
    {
        Ladder = 500,
        Leaf,
        AlertSign,
        LeftSign,
        RightSign,
        Fern,
        Cactus,
        Tree,
        Rod,
        ChainTop,
        Chain,
        ChainBottom,
        Hook,
        RopeTop,
        Rope,
        ChainBottomRope,
        HookRope,
        EndIndex
    }

    // Create constant for color tiles
    public const int CLR_START = (int)Clr.Black;

    public enum Clr
    {
        Black = 900,
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
        return (type >= PLAT_START && type < (int)Plat.EndIndex)
            || type == (int)Func.WallJump;
    }

    public static void LoadTextures(ContentManager content)
    {
        // Load all functional tiles
        for (Func type = (Func)FUNC_START; type < Func.EndIndex; type++)
        {
            textures[(int)type] = content.Load<Texture2D>($"Images/Sprites/Tiles/Functional/{type}");
        }

        // Load all platform tiles
        for (Plat type = (Plat)PLAT_START; type < Plat.EndIndex; type++)
        {
            textures[(int)type] = content.Load<Texture2D>($"Images/Sprites/Tiles/Platform/{type}");
        }

        // Load all decoration tiles
        for (Decor type = (Decor)DECOR_START; type < Decor.EndIndex; type++)
        {
            textures[(int)type] = content.Load<Texture2D>($"Images/Sprites/Tiles/Decoration/{type}");
        }

        // Load all color tiles
        for (Clr type = (Clr)CLR_START; type < Clr.EndIndex; type++)
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