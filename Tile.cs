// Author:          Jason Sun
// File Name:       Tile.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     Handles tile data and logic

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Tile
{
    // Store some draw padding amount
    private const int DRAW_PAD = 2;

    // Create constant for zipline tile type
    public const int ZIPLINE = 1000; // Zipline start and end tiles are from here and onwards in pairs

    // Create constants for if tile is floodable or not
    public const int NOT_FLOODED = -1;

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
        Acid,
        FloodAcid,
        Lava,
        FloodLava,
        Quicksand,
        FloodQuicksand,
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

    /// <summary>
    /// Constructs a tile object
    /// </summary>
    /// <param name="posX">Tile x position</param>
    /// <param name="posY">Tile y position</param>
    /// <param name="type">The tile type</param>
    public Tile(int posX, int posY, int type = (int)Func.Empty)
    {
        Pos = new Point(posX, posY);
        Type = type;
        Rec = new Rectangle(posX * Game1.TILE_SIZE * Game1.PIXEL_SCALE, posY * Game1.TILE_SIZE * Game1.PIXEL_SCALE, Game1.TILE_SIZE * Game1.PIXEL_SCALE, Game1.TILE_SIZE * Game1.PIXEL_SCALE);
    }

    /// <summary>
    /// Copies the tile
    /// </summary>
    /// <returns>Copy of the tile</returns>
    public Tile Copy()
    {
        return new Tile(Pos.X, Pos.Y, Type);
    }

    /// <summary>
    /// Checks if a tile is collidable or not
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>If collidable or not</returns>
    public static bool CanCollide(int type)
    {
        return (type >= PLAT_START && type < (int)Plat.EndIndex)
            || type == (int)Func.WallJump;
    }

    /// <summary>
    /// Checks if a tile is flood type or not
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>If tile is flood type or not</returns>
    public static bool IsFlood(int type)
    {
        return type is (int)Func.Flood or (int)Func.FloodAcid or (int)Func.FloodLava or (int)Func.FloodQuicksand;
    }

    /// <summary>
    /// Checks if a tile is swimmable or not
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>If tile is swimmable or not</returns>
    public static bool IsSwimmable(int type)
    {
        return type is (int)Func.Water or (int)Func.Acid or (int)Func.Lava or (int)Func.Quicksand;
    }

    /// <summary>
    /// Gets the actual type of the tile from stored types
    /// </summary>
    /// <param name="type">Stored map tile types</param>
    /// <param name="isEditing">If editing map or not editing (playing)</param>
    /// <returns>The tile type</returns>
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
        else if (!isEditing)
        {
            if (type == (int)Func.Flood) return (int)Func.Water;
            if (type == (int)Func.FloodAcid) return (int)Func.Acid;
            if (type == (int)Func.FloodLava) return (int)Func.Lava;
            if (type == (int)Func.FloodQuicksand) return (int)Func.Quicksand;
        }

        return type;
    }

    /// <summary>
    /// Gets the texture of a given tile type
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>Texture of the tile</returns>
    public static Texture2D GetTexture(int type)
    {
        if (type == ZIPLINE) return ziplineImg;
        return textures[type];
    }

    /// <summary>
    /// Loads all tile textures
    /// </summary>
    /// <param name="content">ContentManager to load with</param>
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

    /// <summary>
    /// Load editor tile textures
    /// </summary>
    /// <param name="content">ContentManager to load with</param>
    public static void LoadEditorOnlyTiles(ContentManager content)
    {
        SelectImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/SelectTile");
        ziplineImg = content.Load<Texture2D>("Images/Sprites/EditorTiles/ZiplineTile");
    }

    /// <summary>
    /// Draws the tile
    /// </summary>
    /// <param name="spriteBatch">Sprite batch to draw with</param>
    /// <param name="editing">If editing map</param>
    public void Draw(SpriteBatch spriteBatch, bool editing = false)
    {
        // Draw the tile if it can be drawn and should be visible
        int type = GetType(Type, editing);

        if ((type > (int)Func.Empty || type == (int)Func.Barrier || type == (int)Func.Flood)
            && (editing || (type != (int)Func.Start && type != (int)Func.End && type != (int)Func.Barrier && type != (int)Func.Flood))
            && (type < ZIPLINE))
        {
            // Store camera being used to draw
            Camera cam = editing ? Game1.mapEditor.Camera : Game1.player?.Camera;

            // Only draw if camera exists (to be safe)
            if (cam != null)
            {
                // Store the zoom and center of the camera, and calculate dimensions of view
                float zoom = cam.GetZoom();
                Vector2 center = cam.GetPos();

                float viewW = Game1.GAME_W * DRAW_PAD / zoom;
                float viewH = Game1.GAME_H * DRAW_PAD / zoom;

                // Calculate the rectangle that the player can see
                Rectangle viewRect = new Rectangle((int)Math.Floor(center.X - viewW / 2), (int)Math.Floor(center.Y - viewH / 2), (int)Math.Ceiling(viewW), (int)Math.Ceiling(viewH));

                // If the tile is outside of the viewer's sight, don't draw
                if (!viewRect.Intersects(Rec)) return;
            }

            // Draw tile
            spriteBatch.Draw(GetTexture(type), Rec, Color.White);
        }
    }
}