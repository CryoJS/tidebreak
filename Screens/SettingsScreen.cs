// Author:          Jason Sun
// File Name:       SettingsScreen.cs
// Project Name:    Tidebreak
// Creation Date:   June 2, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen for editing player settings

using Gum.Forms.Controls;
using MonoGameGum;
using System;

namespace Tidebreak.Screens
{
    partial class SettingsScreen
    {
        /// <summary>
        /// Sets up screen on creation
        /// </summary>
        partial void CustomInitialize()
        {
            // Disable save btn
            SaveBtn.IsEnabled = false;

            // Update fullscreen toggle
            Fullscreen.IsChecked = Settings.FullScreen;

            // Clamp volume
            MusicVol.Minimum = SfxVol.Minimum = Settings.VOLUME_MIN;
            MusicVol.Maximum = SfxVol.Maximum = Settings.VOLUME_MAX;

            // Update values to current settings
            MusicVol.Value = Settings.MusicVolume;
            SfxVol.Value = Settings.SfxVolume;
            
            // Add bg background toggle logic
            Fullscreen.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                SaveBtn.IsEnabled = true;
            };

            // Add slider logic
            MusicVol.ValueChangeCompleted += (_, _) =>
            {
                SaveBtn.IsEnabled = true;
            };

            // Add slider logic
            SfxVol.ValueChangeCompleted += (_, _) =>
            {
                SaveBtn.IsEnabled = true;
            };

            // Add save button logic
            SaveBtn.Click += (_, _) =>
            {
                // Try to save, if not send error message
                try
                {
                    // Store new settings
                    Settings.FullScreen = (bool)Fullscreen.IsChecked;
                    Settings.MusicVolume = (float)MusicVol.Value;
                    Settings.SfxVolume = (float)SfxVol.Value;

                    // Play sound and save
                    SoundManager.PlayClick();
                    Settings.Save();
                    Settings.Apply();

                    // Disable button after save
                    SaveBtn.IsEnabled = false;
                }
                catch
                {
                    Console.WriteLine("ERROR - Settings failed to save");
                }
            };

            // Add close btn
            ReturnBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                this.RemoveFromRoot();
            };
        }
    }
}
