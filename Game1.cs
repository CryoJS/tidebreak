// NOTE Open MGCB Pipeline Tool: dotnet mgcb-editor ./Content/Content.mgcb

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
    int gameState = MENU;

    // Create input objects (mouse and keyboard)
    KeyboardState kb;
    KeyboardState prevKb;

    MouseState mouse;
    MouseState prevMouse;

    // Create variables to store the screen dimensions
    int screenWidth;
    int screenHeight;

    // Create variables for the tile size and amount of tiles in rows and cols (dimensions) on the screen
    int tileSpanX = 14;
    int tileSpanY = 8;
    int tileSize = 16;

    // Create a variable for pixel art scale
    int pixelScale = 8; // This value gets closest to full HD 1920x1080

    // Create variable for target fps (240 since frame rate is important for platformer games)
    int targetFPS = 240;

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

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        // Update player keyboard
        prevKb = kb;
        kb = Keyboard.GetState();

        // Update mouse state and game pickaxe cursor position
        prevMouse = mouse;
        mouse = Mouse.GetState();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
