// Author:          Jason Sun
// File Name:       SoundManager.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     Manages all the sounds in the game to be easily used around the game

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

static class SoundManager
{
    // Store amount of sfx constants
    private const int BTN_SFXS = 3;
    private const int MAP_SONGS = 38;

    // Store no song constant
    public const string NO_SONG = "No Song";

    // Store default volume constant
    private const float DEFAULT_SONG_VOLUME = 0.1f;

    // Store cooldown for seeking map songs to prevent spamming seek and crashing/freezing
    private const double SEEK_COOLDOWN_MS = 500;

    // Store last seek time
    private static DateTime lastSeek = DateTime.MinValue;

    // Store user volume scalers
    private static float musicScale = 1f;
    private static float sfxScale = 1f;

    // Store all menu sounds
    private static Song lobbyMusic;
    private static Song levelEditorMusic;

    // Store all UI sounds
    private static SoundEffect clickSfx;
    private static SoundEffect loadSfx;

    // Store all gameplay sounds
    private static SoundEffect[] btnSfx = new SoundEffect[3];
    private static int btnIndex = 0;
    private static int btnDelta = 1;

    private static SoundEffect winSfx;
    private static SoundEffect defeatSfx;

    private static SoundEffect underwaterSfx;
    private static SoundEffect walljumpOnSfx;
    private static SoundEffect walljumpOffSfx;
    private static SoundEffect ziplineStartSfx;
    private static SoundEffect ziplineDuringSfx;
    private static SoundEffect ziplineEndSfx;

    // Set up permenant sfx instances
    private static SoundEffectInstance ziplineDuringInstance;
    private static SoundEffectInstance underwaterInstance;

    // Store all map sounds
    private static Dictionary<string, Song> MapSongs = new Dictionary<string, Song>();

    // Store all map song names
    public static readonly string[] mapSongNames =
    [
        "Abandoned Facility",
        "Abandoned Junkyard",
        "Active Volcanic Mines",
        "Beneath The Ruins",
        "Blue Moon",
        "Calamity Kingdom",
        "Casino of Envy",
        "Castle Tides",
        "Cave System",
        "Central Mass Array",
        "Chaoz Japan",
        "Construction Thrill",
        "Cyberpunk District",
        "Dark Sci-Forest",
        "Decaying Silo",
        "Fallen",
        "Familiar Ruins",
        "Flood Island",
        "Gloomy Manor",
        "Havoc Highlands",
        "Ignis Peaks",
        "Lost Desert",
        "Lost Woods",
        "Luminance",
        "Magmatic Mines",
        "Mars Stations",
        "Mirage Saloon",
        "Oriental Grove",
        "Poisonous Chasm",
        "Rustic Jungle",
        "Sandswept Ruins",
        "Sinking Ship",
        "Snowy Stronghold",
        "Splendid China Wall",
        "Sub-Zerosphere",
        "Undersea Facility",
        "Whirlwind Wasteland",
        "Wildwood Waterways"
    ];

    // Store all volume of all songs and sfx
    private static Dictionary<string, float> volume = new Dictionary<string, float>();

    /// <summary>
    /// Loads all the game sounds
    /// </summary>
    /// <param name="content">Content manager to load with</param>
    public static void Load(ContentManager content)
    {
        // Set songs to repeat
        MediaPlayer.IsRepeating = true;

        // Load all menu sounds
        lobbyMusic = content.Load<Song>("Audio/Menu/Lobby");
        levelEditorMusic = content.Load<Song>("Audio/Menu/Level Editor");

        // Load all UI sounds
        clickSfx = content.Load<SoundEffect>("Audio/UI/Click");
        loadSfx = content.Load<SoundEffect>("Audio/UI/Load");

        // Load all gameplay sounds
        for (int i = 0; i < BTN_SFXS; i++)
        {
            btnSfx[i] = content.Load<SoundEffect>($"Audio/Gameplay/Btn{i}");
        }

        winSfx = content.Load<SoundEffect>("Audio/Gameplay/Win");
        defeatSfx = content.Load<SoundEffect>("Audio/Gameplay/DefeatBg");

        underwaterSfx = content.Load<SoundEffect>("Audio/Gameplay/Underwater");
        walljumpOnSfx = content.Load<SoundEffect>("Audio/Gameplay/WalljumpOn");
        walljumpOffSfx = content.Load<SoundEffect>("Audio/Gameplay/WalljumpOff");
        ziplineStartSfx = content.Load<SoundEffect>("Audio/Gameplay/ZiplineStart");
        ziplineDuringSfx = content.Load<SoundEffect>("Audio/Gameplay/ZiplineDuring");
        ziplineEndSfx = content.Load<SoundEffect>("Audio/Gameplay/ZiplineEnd");

        // Load all map songs
        for (int i = 0; i < MAP_SONGS; i++)
        {
            MapSongs[mapSongNames[i]] = content.Load<Song>($"Audio/Map/{mapSongNames[i]}");
        }

        // Set all song volume to be quieter
        MediaPlayer.Volume = DEFAULT_SONG_VOLUME;

        // Set custom sound volume
        volume[lobbyMusic.Name] = 0.1f;
        volume[levelEditorMusic.Name] = 0.1f;
        volume[clickSfx.Name] = 0.5f;

        volume[walljumpOnSfx.Name] = 0.1f;
        volume[walljumpOffSfx.Name] = 0.3f;
        volume[ziplineStartSfx.Name] = 0.1f;
        volume[ziplineEndSfx.Name] = 0.1f;
        for (int i = 0; i < btnSfx.Length; i++) volume[btnSfx[i].Name] = 0.6f;
        
        volume[loadSfx.Name] = 0.3f;
        volume[winSfx.Name] = 0.6f;
        volume[defeatSfx.Name] = 1f;

        // Edit custom song volumes if needed
        try
        {
            volume[MapSongs["Calamity Kingdom"].Name] = 0.15f;
            volume[MapSongs["Central Mass Array"].Name] = 0.2f;
            volume[MapSongs["Havoc Highlands"].Name] = 0.15f;
            volume[MapSongs["Mars Stations"].Name] = 0.2f;
        }
        catch
        {
            Console.WriteLine("ERROR - Failed to change outlier song volumes, check if song names are still valid");
        }

        // Set up zipline during instance with volume
        ziplineDuringInstance = ziplineDuringSfx.CreateInstance();
        ziplineDuringInstance.Volume = 0.1f;

        // Set up underwater instance
        underwaterInstance = underwaterSfx.CreateInstance();
        underwaterInstance.Volume = 0.4f;
    }

    /// <summary>
    /// Plays a sound effect
    /// </summary>
    /// <param name="sfx">Sound effect</param>
    public static void Play(SoundEffect sfx)
    {
        // Create instance and sets volume if custom volume is set
        SoundEffectInstance instance = sfx.CreateInstance();
        instance.Volume = (volume.TryGetValue(sfx.Name, out float value) ? value : 1f) * sfxScale;
        instance.Play();
    }

    /// <summary>
    /// Start to play a song
    /// </summary>
    /// <param name="song">Song to play</param>
    /// <param name="restart">If forcing restart if same song</param>
    public static void Play(Song song, bool restart = false)
    {
        // Change the volume if custom volume is set, otherwise set it back to default
        MediaPlayer.Volume = (volume.TryGetValue(song.Name, out float value) ? value : DEFAULT_SONG_VOLUME) * musicScale;
        if (restart || song != MediaPlayer.Queue.ActiveSong) MediaPlayer.Play(song);
    }

    /// <summary>
    /// Plays a click sfx
    /// </summary>
    public static void PlayClick() => Play(clickSfx);
    
    /// <summary>
    /// Plays a load sfx
    /// </summary>
    public static void PlayLoad() => Play(loadSfx);
    
    /// <summary>
    /// Plays a button sfx (bouncing around to different sound effects)
    /// </summary>
    public static void PlayButton(int index = 0)
    {
        // If index is out of bounds, flip direction and change index (seesaw effect)
        if (btnIndex + btnDelta >= btnSfx.Length || btnIndex + btnDelta < 0) btnDelta *= -1;
        btnIndex += btnDelta;

        // Play that button sound
        Play(btnSfx[btnIndex]);
    }
    
    /// <summary>
    /// Plays a win sfx
    /// </summary>
    public static void PlayWin() => Play(winSfx);
    
    /// <summary>
    /// Plays a death sfx
    /// </summary>
    public static void PlayDeath() => Play(defeatSfx);

    /// <summary>
    /// Plays underwater sfx
    /// </summary>
    public static void PlayUnderwater()
    {
        if (underwaterInstance.State != SoundState.Playing) underwaterInstance.Play();
    }
    
    /// <summary>
    /// Stops playing underwater sfx
    /// </summary>
    public static void StopUnderwater()
    {
        if (underwaterInstance.State == SoundState.Playing) underwaterInstance.Stop();
    }
    
    /// <summary>
    /// Plays wall jump on sfx
    /// </summary>
    public static void PlayWalljumpOn() => Play(walljumpOnSfx);
    
    /// <summary>
    /// Plays wall jump off sfx
    /// </summary>
    public static void PlayWalljumpOff() => Play(walljumpOffSfx);
    
    /// <summary>
    /// Play zipline attach sfx
    /// </summary>
    public static void PlayZiplineStart() => Play(ziplineStartSfx);
    
    /// <summary>
    /// Play zipline sliding sfx
    /// </summary>
    public static void PlayZiplineDuring() 
    {
        if (ziplineDuringInstance.State != SoundState.Playing) ziplineDuringInstance.Play();
    }
    
    /// <summary>
    /// Play zipline detatch sfx
    /// </summary>
    /// <param name="onlyStop">To only stop and not play detach</param>
    public static void PlayZiplineEnd(bool onlyStop = false)
    {
        if (ziplineDuringInstance.State == SoundState.Playing) ziplineDuringInstance.Stop();
        if (!onlyStop) Play(ziplineEndSfx);
    }

    /// <summary>
    /// Play lobby music
    /// </summary>
    public static void PlayLobbyMusic() => Play(lobbyMusic);
    
    /// <summary>
    /// Play level editor music
    /// </summary>
    public static void PlayLevelEditorMusic() => Play(levelEditorMusic);

    /// <summary>
    /// Play map song music
    /// </summary>
    /// <param name="name">Name of the song of the map</param>
    public static void PlayMapSong(string name)
    {
        // If song exists, play it
        if (MapSongs.TryGetValue(name, out Song value))
        {
            Play(value, true);
            return;
        }

        // If song doesn't exist, stop music
        StopMusic();
    }

    /// <summary>
    /// Stop playing map music
    /// </summary>
    public static void StopMusic() => MediaPlayer.Stop();

    /// <summary>
    /// Play a map song at certain position
    /// </summary>
    /// <param name="name"></param>
    /// <param name="position"></param>
    public static void SeekMapSong(string name, TimeSpan position)
    {
        // Quit if spamming seek, otherwise move on and store current date
        if ((DateTime.Now - lastSeek).TotalMilliseconds < SEEK_COOLDOWN_MS) return;
        lastSeek = DateTime.Now;

        // If song exists, seek to that position (clamped to song duration) (try to do it, if failed just ignore)
        if (MapSongs.TryGetValue(name, out Song song))
        {
            TimeSpan clamped = TimeSpan.FromSeconds(Math.Clamp(position.TotalSeconds, 0, song.Duration.TotalSeconds - 0.1));
            try
            {
                MediaPlayer.Play(song, clamped);
            }
            catch
            {
                Console.WriteLine("ERROR - Failed to fast forward/rewind, wait and try again");
            }
        }
    }

    /// <summary>
    /// Set a new song volume scaling
    /// </summary>
    /// <param name="scale">New scaling for song volume</param>
    public static void SetMusicScale(float scale)
    {
        // Update music scaling to new factor
        musicScale = scale;

        // Re-apply to currently playing song
        if (MediaPlayer.Queue.ActiveSong != null)
        {
            string name = MediaPlayer.Queue.ActiveSong.Name;
            MediaPlayer.Volume = (volume.TryGetValue(name, out float vol) ? vol : DEFAULT_SONG_VOLUME) * musicScale;
        }
    }

    /// <summary>
    /// Set a new sound effect volume scaling
    /// </summary>
    /// <param name="scale">New scaling for sfx volume</param>
    public static void SetSfxScale(float scale)
    {
        // Update sfx scaling to new factor
        sfxScale = scale;

        // Re-apply to looping instances
        ziplineDuringInstance.Volume = 0.1f * sfxScale;
        underwaterInstance.Volume = 0.4f * sfxScale;
    }
}