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

    // Store all map sounds // REVIEW im allowed to use dicts right?
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

    public static void Play(SoundEffect sfx)
    {
        // Create instance and sets volume if custom volume is set
        SoundEffectInstance instance = sfx.CreateInstance();
        instance.Volume = volume.TryGetValue(sfx.Name, out float value) ? value : 1f;
        instance.Play();
    }

    public static void Play(Song song, bool restart = false)
    {
        // Change the volume if custom volume is set, otherwise set it back to default
        MediaPlayer.Volume = volume.TryGetValue(song.Name, out float value) ? value : DEFAULT_SONG_VOLUME;
        if (restart || song != MediaPlayer.Queue.ActiveSong) MediaPlayer.Play(song);
    }

    public static void PlayClick() => Play(clickSfx);
    public static void PlayLoad() => Play(loadSfx);
    public static void PlayButton(int index = 0)
    {
        // If index is out of bounds, flip direction and change index (seesaw effect)
        if (btnIndex + btnDelta >= btnSfx.Length || btnIndex + btnDelta < 0) btnDelta *= -1;
        btnIndex += btnDelta;

        // Play that button sound
        Play(btnSfx[btnIndex]);
    }
    public static void PlayWin() => Play(winSfx);
    public static void PlayDeath() => Play(defeatSfx);

    public static void PlayUnderwater()
    {
        if (underwaterInstance.State != SoundState.Playing) underwaterInstance.Play();
    }
    public static void StopUnderwater()
    {
        if (underwaterInstance.State == SoundState.Playing) underwaterInstance.Stop();
    }
    public static void PlayWalljumpOn() => Play(walljumpOnSfx);
    public static void PlayWalljumpOff() => Play(walljumpOffSfx);
    public static void PlayZiplineStart() => Play(ziplineStartSfx);
    public static void PlayZiplineDuring() 
    {
        if (ziplineDuringInstance.State != SoundState.Playing) ziplineDuringInstance.Play();
    }
    public static void PlayZiplineEnd(bool onlyStop = false)
    {
        if (ziplineDuringInstance.State == SoundState.Playing) ziplineDuringInstance.Stop();
        if (!onlyStop) Play(ziplineEndSfx);
    }

    public static void PlayLobbyMusic() => Play(lobbyMusic);
    public static void PlayLevelEditorMusic() => Play(levelEditorMusic);

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

    public static void StopMusic() => MediaPlayer.Stop();

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
                Console.WriteLine("ERROR - Failed to fast forward/rewind, wait a bit and try again");
            }
        }
    }
}