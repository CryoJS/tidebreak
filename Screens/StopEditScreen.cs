// Author:          Jason Sun
// File Name:       StopEditScreen.cs
// Project Name:    Tidebreak
// Creation Date:   May 17, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen for the popup when trying to stop editing without saving

using Gum.Forms.Controls;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class StopEditScreen
    {
        /// <summary>
        /// Sets up screen on creation
        /// </summary>
        partial void CustomInitialize()
        {
            // Ensure no editing is allowed
            Game1.mapEditor.IsEditing = false;

            // Add logic for button to close popup
            CloseBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // Turn editing back on
                Game1.mapEditor.IsEditing = true;

                this.RemoveFromRoot();
            };

            // Add logic for button to save and leave
            SaveBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // Save
                Game1.mapEditor.Save();
                Game1.SaveMaps();

                // Leave
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };

            // Add logic for button to leave without saving
            ExitBtn.Click += (_, _) =>
            {
                // Play click sound and clear screens, open map select screen
                SoundManager.PlayClick();
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };
        }
    }
}
