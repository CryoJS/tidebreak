// NOTE Open MGCB Pipeline Tool: dotnet mgcb-editor ./Content/Content.mgcb

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameUtility;

using MonoGameGum;
using Tidebreak.Screens;
using Gum.DataTypes;
using System.ComponentModel.Design.Serialization;

namespace Tidebreak;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    GumService GumUI => GumService.Default;

    // Create random object
    public static Random rng = new Random();

    // Create game state constants
    public const int MENU = 0;
    public const int SELECT_MAP = 1;
    public const int CREATE_MAP = 2;
    public const int PLAY_MAP = 3;
    public const int EDIT_MAP = 4;
    public const int END_MAP = 5;
    public const int EXIT = 6;

    // Create constants for direction (negative is left, positive is right)
    public const int LEFT_DIRECTION = -1;
    public const int RIGHT_DIRECTION = 1;

    // Create game display constants
    public const int TILE_SPAN_X = 14;     // Tiles spanning horizontally
    public const int TILE_SPAN_Y = 8;      // Tiles spanning vertically
    public const int TILE_SIZE = 16;       // Pixel size of each tile (before scaling)

    public const int PIXEL_SCALE = 8;      // This scale value gets closest to full HD 1920x1080
    public const int TARGET_FPS = 240;     // Target high frame rate, important for platformer games

    // Set the starting game state to be the menu state
    public static int gameState = MENU;

    // Create variables for file IO
    private static StreamReader inFile;
    private static StreamWriter outFile;

    // Create input objects (mouse and keyboard)
    private KeyboardState kb;
    private KeyboardState prevKb;

    private MouseState mouse;
    private MouseState prevMouse;

    // Create variables to store the screen dimensions
    private int screenWidth;
    private int screenHeight;

    // Store cursor textures
    Texture2D cursorImg;
    Texture2D cursorPressedImg;

    // Store all saved maps
    internal static List<Map> maps = new List<Map>();
    internal static int currentMap = 0;

    // Store player and player's camera
    internal static Player player;
    internal static Camera camera;

    // Store all needed screens
    internal static PlayScreen playScreen;

    // Store if the game is paused
    internal static bool paused;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Set the preferred resolution
        _graphics.PreferredBackBufferWidth = TILE_SPAN_X * TILE_SIZE * PIXEL_SCALE; // Assuming 1792
        _graphics.PreferredBackBufferHeight = TILE_SPAN_Y * TILE_SIZE * PIXEL_SCALE; // Assuming 1024

        Console.WriteLine("Initializing game with resolution: " + _graphics.PreferredBackBufferWidth + "x" + _graphics.PreferredBackBufferHeight);

        // Set game FPS to target FPS, turn off VSync, and try to ensure equal frame time
        _graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / TARGET_FPS);

        // Apply new resolution changes
        _graphics.ApplyChanges();

        // Store the width and height of the screen
        screenWidth = _graphics.GraphicsDevice.Viewport.Width;
        screenHeight = _graphics.GraphicsDevice.Viewport.Height;

        // Hide the cursor (using custom texture)
        IsMouseVisible = false;

        // Initialize GUM (UI library)
        GumUI.Initialize(this, "Gum/TidebreakGum.gumx");
        TitleScreen startingScreen = new TitleScreen();
        startingScreen.AddToRoot();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load cursor textures
        cursorImg = Content.Load<Texture2D>($"Images/Sprites/Icons/Cursor");
        cursorPressedImg = Content.Load<Texture2D>($"Images/Sprites/Icons/CursorPressed");

        // Load all tile textures
        for (int i = 0; i < Tile.TYPE_AMOUNT; i++)
        {
            Tile.tileTextures[i] = Content.Load<Texture2D>($"Images/Sprites/Tiles/Tile{i}");
        }

        // Create player (load player textures and animations)
        player = new Player(Content);

        // Initialize the camera object
        camera = new Camera(GraphicsDevice.Viewport);

        // Load in all maps
        LoadMaps();

        // FIXME Add fake maps to test map select
        for (int i = 0; i < 20; i++)
        {
            maps.Add(new Map($"Test {i+1}", "idek", 1.0f, 1, 1));
        }
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
                // TODO
                break;

            case SELECT_MAP:
                break;

            case CREATE_MAP:
                break;

            case PLAY_MAP:
                if (!paused)
                {
                    // Update map and player
                    maps[currentMap].Update(gameTime);
                    player.Update(gameTime, kb, prevKb, camera, maps[currentMap]);

                    // Update time passed UI (for safety make it optional)
                    playScreen?.Update();

                    // If the player died open game over screen
                    if (player.isGameOver)
                    {
                        // TODO gotta make game over screen
                        MapSelectScreen newScreen = new MapSelectScreen();
                        GumService.Default.Root.Children.Clear();
                        newScreen.AddToRoot();
                        gameState = SELECT_MAP;
                    }
                }
                break;

            case EDIT_MAP:
                break;

            case END_MAP:
                break;

            case EXIT:
                Exit();
                break;
        }

        GumUI.Update(gameTime);
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

            case PLAY_MAP:
                GraphicsDevice.Clear(Color.CornflowerBlue); // TODO let maps have bg color (+ documentation)
                cameraSpriteBatchBegin();

                maps[currentMap].Draw(_spriteBatch);
                player.Draw(_spriteBatch);

                _spriteBatch.End();
                break;

            case EDIT_MAP:
                break;

            case END_MAP:
                break;
        }

        // Draw the UI made with the GUM library
        GumUI.Draw();

        // Draw the cursor over everything
        _spriteBatch.Begin();
        _spriteBatch.Draw(mouse.LeftButton == ButtonState.Pressed ? cursorPressedImg : cursorImg, mouse.Position.ToVector2(), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void cameraSpriteBatchBegin()
    {
        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
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
                maps.Last().Load(inFile, _graphics.GraphicsDevice);
            }
        }
        catch
        {
            // TODO load locked/default maps
        }
    }

    public static void ReturnToMenu()
    {
        // Remove all screens and go to menu (title screen)
        TitleScreen newScreen = new TitleScreen();
        GumService.Default.Root.Children.Clear();
        newScreen.AddToRoot();

        // Change gamestate
        gameState = MENU;
    }

    public static string FormatTime(float seconds, bool includeMs = true)
    {
        if (includeMs) return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss\:fff");
        return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
    }
}
