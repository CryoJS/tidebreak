using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

static class Settings
{
    // Store file name
    private const string FILE_NAME = "UserSettings.txt";

    // Store constraints for volume
    public const float VOLUME_MIN = 0;
    public const float VOLUME_MAX = 1;

    // Store defaults
    private const bool DEFAULT_FULLSCREEN = true;
    private const float DEFAULT_MUSIC_VOLUME = 1f;
    private const float DEFAULT_SFX_VOLUME = 1f;

    // Store user settings
    public static bool FullScreen { get; set; }
    public static float MusicVolume { get; set; }
    public static float SfxVolume { get; set; }

    public static void Load()
    {
        // Try to load maps, if failed, load defaults
        try
        {
            // Load the file
            Game1.inFile = new StreamReader(FILE_NAME);

            // Take in the settings
            FullScreen = Convert.ToBoolean(Game1.inFile.ReadLine());
            MusicVolume = Convert.ToSingle(Game1.inFile.ReadLine());
            SfxVolume = Convert.ToSingle(Game1.inFile.ReadLine());
        }
        catch
        {
            // If any failures, assign default settings and save
            FullScreen = DEFAULT_FULLSCREEN;
            MusicVolume = DEFAULT_MUSIC_VOLUME;
            SfxVolume = DEFAULT_SFX_VOLUME;

            Save();

            // Send error message
            Console.WriteLine("ERROR - Settings failed to load");
        }
        finally
        {
            // Close file if opened
            Game1.inFile?.Close();

            // Apply settings
            Apply();
        }
    }

    public static void Save()
    {
        try
        {
            // Create a new file to put the settings in
            Game1.outFile = File.CreateText(FILE_NAME);

            // Store all the current settings
            Game1.outFile.WriteLine(FullScreen);
            Game1.outFile.WriteLine(MusicVolume);
            Game1.outFile.WriteLine(SfxVolume);
        }
        catch
        {
            // Send error message
            Console.WriteLine("ERROR - Settings failed to save");
        }
        finally
        {
            // Close file if opened
            Game1.outFile?.Close();
        }
    }

    public static void Apply()
    {
        // Apply fullscreen (borderless) if toggled on
        if (FullScreen)
        {
            // Save windowed size and go borderless fullscreen at monitor resolution
            Game1._graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Game1._graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            
            Game1._graphics.HardwareModeSwitch = false;
            Game1._graphics.IsFullScreen = true;
        }
        else
        {
            // Apply default resolution otherwise
            ApplyDefaultRes();
            Game1._graphics.HardwareModeSwitch = true;
            Game1._graphics.IsFullScreen = false;
        }

        // Apply fullscreen graphics changes
        Game1._graphics.IsFullScreen = FullScreen;
        Game1._graphics.ApplyChanges();

        // Apply volume changes
        SoundManager.SetMusicScale(MusicVolume);
        SoundManager.SetSfxScale(SfxVolume);
    }

    public static void ApplyDefaultRes()
    {
        // Set the preferred resolution
        Game1._graphics.PreferredBackBufferWidth = Game1.TILE_SPAN_X * Game1.TILE_SIZE * Game1.PIXEL_SCALE;
        Game1._graphics.PreferredBackBufferHeight = Game1.TILE_SPAN_Y * Game1.TILE_SIZE * Game1.PIXEL_SCALE;

        // Create render target at fixed game resolution
        Game1.gameRenderTarget = new RenderTarget2D(Game1._graphics.GraphicsDevice, Game1._graphics.PreferredBackBufferWidth, Game1._graphics.PreferredBackBufferHeight);
    }
}