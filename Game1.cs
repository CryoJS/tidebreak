// NOTE Open MGCB Pipeline Tool: dotnet mgcb-editor ./Content/Content.mgcb

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Camera;
using GameUtility;

namespace Tidebreak;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Create random object
    public static Random rng = new Random();

    // Create game state constants
    private const int MENU = 0;
    private const int SELECT_MAP = 1;
    private const int CREATE_MAP = 2;
    private const int EDIT_MAP = 3;
    private const int PLAY_MAP = 4;
    private const int END_MAP = 5;

    // Create constants for direction (negative is left, positive is right)
    private const int LEFT_DIRECTION = -1;
    private const int RIGHT_DIRECTION = 1;

    // Set the starting game state to be the menu state
    private int gameState = MENU;

    // Create variables for file IO
    private static StreamReader inFile;
    private static StreamWriter outFile;

    // Create input objects (mouse and keyboard)
    private KeyboardState kb;
    private KeyboardState prevKb;

    private MouseState mouse;
    private MouseState prevMouse;

    // Create viewport camera
    Cam2D camera;

    // Create variables to store the screen dimensions
    private int screenWidth;
    private int screenHeight;

    // Create variables for the tile size and amount of tiles in rows and cols (dimensions) on the screen
    private int tileSpanX = 14;
    private int tileSpanY = 8;
    private int tileSize = 16;

    // Create a variable for pixel art scale
    private int pixelScale = 8; // This value gets closest to full HD 1920x1080

    // Create variable for target fps (240 since frame rate is important for platformer games)
    private int targetFPS = 240;

    // Store all saved maps
    private List<Map> maps = new List<Map>();
    private int currentMap = 0;

    // Store basic player data
    Texture2D playerImg;
    Rectangle playerRec;
    Vector2 playerPos;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Set the preferred resolution
        _graphics.PreferredBackBufferWidth = tileSpanX * tileSize * pixelScale;
        _graphics.PreferredBackBufferHeight = tileSpanY * tileSize * pixelScale;

        Console.WriteLine("Initializing game with resolution: " + _graphics.PreferredBackBufferWidth + "x" + _graphics.PreferredBackBufferHeight);

        // Set game FPS to target FPS, turn off VSync, and try to ensure equal frame time
        _graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / targetFPS);

        // Apply new resolution changes
        _graphics.ApplyChanges();

        // Store the width and height of the screen
        screenWidth = _graphics.GraphicsDevice.Viewport.Width;
        screenHeight = _graphics.GraphicsDevice.Viewport.Height;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load all tile textures
        for (int i = 0; i < Tile.TYPE_AMOUNT; i++)
        {
            Tile.tileTextures[i] = Content.Load<Texture2D>($"Images/Sprites/Tiles/Tile{i}");
        }

        // Load in all maps
        LoadMaps();
    }

    protected override void Update(GameTime gameTime)
    {
        // Update player keyboard
        prevKb = kb;
        kb = Keyboard.GetState();

        // Update mouse state and game pickaxe cursor position
        prevMouse = mouse;
        mouse = Mouse.GetState();

        // Perform update logic based on the current game state
        switch (gameState)
        {
            case MENU:
                break;

            case SELECT_MAP:
                break;

            case CREATE_MAP:
                break;

            case EDIT_MAP:
                break;

            case PLAY_MAP:
                maps[currentMap].Update();
                break;

            case END_MAP:
                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // Perform update logic based on the current game state
        switch (gameState)
        {
            case MENU:
                break;

            case SELECT_MAP:
                break;

            case CREATE_MAP:
                break;

            case EDIT_MAP:
                break;

            case PLAY_MAP:
                GraphicsDevice.Clear(Color.CornflowerBlue);
                maps[currentMap].Draw(_spriteBatch);
                break;

            case END_MAP:
                break;
        }

        base.Draw(gameTime);
    }

    private void LoadMaps()
    {
        // Try to load maps, if failed, load defaults
        try
        {
            // Load the file
            inFile = new StreamReader("SavedMaps.txt");

            // Load maps until none left
            while (!inFile.EndOfStream)
            {
                maps.Add(new Map());
                maps.Last().Load(inFile);
            }
        }
        catch
        {
            // TODO load locked/default maps
        }
    }
}
