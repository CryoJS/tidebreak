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
using MonoGameGum.GueDeriving;
using MonoGameAndGum.Renderables;
using Gum.Forms.Controls;

namespace Tidebreak;

public class Game1 : Game
{
    public static GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    GumService GumUI => GumService.Default;

    // Create random object
    public static Random rng = new Random();

    // Create game state constants // FIXME (can prob remove if not using much?)
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

    // Create string limit constants
    public const int MAX_LENGTH_LONG = 80;
    public const int MAX_LENGTH = 16;
    public const int MAX_SHORT = 12;

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
    public static int ScreenWidth { get; private set; }
    public static int ScreenHeight { get; private set; }

    // Store cursor textures
    Texture2D cursorImg;
    Texture2D cursorPressedImg;

    // Store all saved maps
    internal static List<Map> maps = new List<Map>();
    internal static Map currentMap;

    // Store player and player's camera
    internal static Player player;

    // Store map editor
    internal static MapEditor mapEditor = new MapEditor();

    // Store all needed screens
    internal static PlayScreen playScreen;
    internal static PauseScreen pauseScreen;
    internal static EditScreen editScreen;

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

        // Set game FPS to target FPS, turn off VSync, and try to ensure equal frame time
        _graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / TARGET_FPS);

        // Apply new resolution changes
        _graphics.ApplyChanges();

        // Store the width and height of the screen
        ScreenWidth = _graphics.GraphicsDevice.Viewport.Width;
        ScreenHeight = _graphics.GraphicsDevice.Viewport.Height;

        // Display goal resolution and current resolution
        Console.WriteLine($"Initializing game with resolution: {_graphics.PreferredBackBufferWidth}x{_graphics.PreferredBackBufferHeight} | Current resolution: {ScreenWidth}x{ScreenHeight}");

        // Hide the cursor (using custom texture)
        IsMouseVisible = false;

        // Initialize GUM (UI library)
        AposShapeRuntime.RegisterRuntimeTypes();
        GumUI.Initialize(this, "Gum/TidebreakGum.gumx");
        ShapeRenderer.Self.Initialize();

        // Initialize the starting screen
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
        for (int i = 0; i < Tile.TEXTURE_TYPE_AMOUNT; i++)
        {
            Tile.textures[i] = Content.Load<Texture2D>($"Images/Sprites/Tiles/Tile{i}");
        }

        // Load editor only tile textures
        Tile.LoadEditorOnlyTiles(Content);

        // Create player (load player textures and animations)
        player = new Player(Content);

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
            case PLAY_MAP:
                if (!paused)
                {
                    // Update map and player
                    currentMap?.Update(gameTime);
                    player.Update(gameTime, kb, prevKb, currentMap);

                    // Update play screen dynamic UI
                    playScreen?.Update(kb, prevKb);
                }
                else
                {
                    // Update pause screen UI (for inputs)
                    pauseScreen?.Update(kb, prevKb);
                }
                break;

            case EDIT_MAP:
                mapEditor.Update(gameTime, kb, prevKb, mouse, prevMouse);
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
            case PLAY_MAP:
                // Clear screen, start drawing
                GraphicsDevice.Clear(Color.CornflowerBlue); // TODO let maps have bg color (+ documentation)
                CameraSpriteBatchBegin(player.Camera);

                // Draw the map and player
                currentMap?.Draw(_spriteBatch);
                player.Draw(_spriteBatch);

                _spriteBatch.End();
                break;

            case EDIT_MAP:
                // Clear screen, start drawing
                GraphicsDevice.Clear(Color.CornflowerBlue); // TODO let maps have bg color (+ documentation)
                CameraSpriteBatchBegin(mapEditor.Camera);
                
                // Draw the map
                mapEditor.Draw(_spriteBatch, mouse);
                _spriteBatch.End();
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

    private void CameraSpriteBatchBegin(Camera camera)
    {
        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
    }

    private static void LoadMaps()
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
            Console.WriteLine("ERROR - Maps failed to load, loading defaults.");
        }
    }

    public static void ReturnToMenu()
    {
        // Remove all screens and go to menu (title screen)
        GumService.Default.Root.Children.Clear();
        new TitleScreen().AddToRoot();

        // Change gamestate
        gameState = MENU;
    }

    internal static void PlayMap(Map nextMap)
    {
        // Change gamestate (and map)
        currentMap = nextMap;
        gameState = PLAY_MAP;
        
        // Change screen
        GumService.Default.Root.Children.Clear();
        new PlayScreen().AddToRoot();

        // Load map
        currentMap.Start(player);
    }

    public static string FormatTime(float seconds, bool includeMs = true)
    {
        if (includeMs) return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss\:fff");
        return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
    }

    public static float ExpSmoothing(GameTime gameTime, float speed)
    {
        // Calculate % value to get exponentially closer if farther with no affect from frame rate (source: https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/)
        return 1 - MathF.Exp(-speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    // NOTE: Takes world position
    public static Point CalcTile(Vector2 pos)
    {
        return new Point((int)Math.Floor(pos.X / (TILE_SIZE * PIXEL_SCALE)), (int)Math.Floor(pos.Y / (TILE_SIZE * PIXEL_SCALE)));
    }

    public static void FloatOnlyHandler(object sender, TextCompositionEventArgs args)
    {
        // Store current input and calculate new input
        TextBox textBox = (TextBox)sender;
        string newText = textBox.Text.Insert(textBox.CaretIndex, args.Text);

        // Check if new input is valid using try parse
        args.Handled = !float.TryParse(newText, out _);
    }

    public static void IntegerOnlyHandler(object sender, TextCompositionEventArgs args)
    {
        args.Handled = args.Text.Any(c => !char.IsDigit(c));
    }
}