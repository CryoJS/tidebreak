// Author:          Jason Sun
// File Name:       SongSelectScreen.cs
// Project Name:    Tidebreak
// Creation Date:   June 2, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen for the song selection menu (when editing map settings)

using System;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Media;
using Tidebreak.Components;
using static Tidebreak.Components.Elements.Icon;

namespace Tidebreak.Screens
{
    partial class SongSelectScreen
    {
        // Store play/pause icon states for easy switching
        private const IconCategory PLAY_ICON = IconCategory.Play;
        private const IconCategory PAUSE_ICON = IconCategory.Pause;

        // Store default scroll speed for song list
        private const int DEFAULT_SCROLL_SPEED = 50;
        
        // Store fast skip amount (seconds)
        private const int SKIP_AMOUNT = 5;

        // Store current active song row for
        private SongRow activeSongRow = null;

        /// <summary>
        /// Sets up screen on creation
        /// </summary>
        partial void CustomInitialize()
        {
            // Stop playing music if we were (and any sfx)
            SoundManager.StopMusic();
            SoundManager.StopUnderwater();
            SoundManager.PlayZiplineEnd(true);

            // Increase scroll speed
            SongList.VerticalScrollBarInstance.SmallChange = DEFAULT_SCROLL_SPEED;

            // Add a row for each map song
            foreach (string name in SoundManager.mapSongNames)
            {
                // Create new row and update the song title text
                SongRow row = new SongRow();
                row.TitleText.Text = name;
                row.FocusedIndicator.Visible = false;

                // Add logic for play/pause button
                row.PlayBtn.Click += (_, _) =>
                {
                    SoundManager.PlayClick();

                    // If this row is already the active one, toggle pause/play
                    if (activeSongRow == row)
                    {
                        if (MediaPlayer.State == MediaState.Playing)
                        {
                            MediaPlayer.Pause();
                            row.PlayBtnIcon.IconCategoryState = PLAY_ICON;
                        }
                        else
                        {
                            MediaPlayer.Resume();
                            row.PlayBtnIcon.IconCategoryState = PAUSE_ICON;
                        }
                    }
                    else
                    {
                        // Deactivate old row indicator (and ensure play icon is correct)
                        if (activeSongRow != null)
                        {
                            activeSongRow.FocusedIndicator.Visible = false;
                            activeSongRow.PlayBtnIcon.IconCategoryState = PLAY_ICON;
                        }

                        // Set this row as active and play song
                        activeSongRow = row;
                        row.FocusedIndicator.Visible = true;
                        row.PlayBtnIcon.IconCategoryState = PAUSE_ICON;
                        SoundManager.PlayMapSong(name);
                    }
                };

                // Fast forward 5 seconds
                row.FastForwardBtn.Click += (_, _) =>
                {
                    // If this row isn't active, ignore
                    if (activeSongRow != row) return;

                    // Calculate new position and restart song at that position
                    TimeSpan newPos = MediaPlayer.PlayPosition + TimeSpan.FromSeconds(SKIP_AMOUNT);
                    SoundManager.SeekMapSong(name, newPos);
                };

                // Fast backward 5 seconds
                row.FastBackBtn.Click += (_, _) =>
                {
                    // If this row isn't active, ignore
                    if (activeSongRow != row) return;

                    // Calculate new position and restart song at that position
                    TimeSpan newPos = MediaPlayer.PlayPosition - TimeSpan.FromSeconds(SKIP_AMOUNT);
                    SoundManager.SeekMapSong(name, newPos < TimeSpan.Zero ? TimeSpan.Zero : newPos);
                };

                // Restart song
                row.RestartBtn.Click += (_, _) =>
                {
                    if (activeSongRow != row) return;
                    SoundManager.PlayMapSong(name);
                };

                // Save song to map
                row.SaveBtn.Click += (_, _) =>
                {
                    SoundManager.PlayClick();
                    Game1.currentMap.Song = name;

                    Game1.currentMap.UpdateModified();
                    Game1.SaveMaps();
                };

                // Add finalized song row into list of songs
                SongList.AddChild(row.Visual);
            }

            // Add logic for no music button
            NoSongBtn.Click += (_, _) =>
            {
                // Stop music preview
                SoundManager.PlayClick();
                SoundManager.StopMusic();

                // Save no song choice
                Game1.currentMap.Song = SoundManager.NO_SONG;
                Game1.currentMap.UpdateModified();
                Game1.SaveMaps();
            };

            // Add logic for button to close popup
            ReturnBtn.Click += (_, _) =>
            {
                // Play click sound and start lobby music again
                SoundManager.PlayClick();
                SoundManager.PlayLobbyMusic();
                this.RemoveFromRoot();
            };
        }
    }
}
