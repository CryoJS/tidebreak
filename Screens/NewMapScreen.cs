// Author:          Jason Sun
// File Name:       NewMapScreen.cs
// Project Name:    Tidebreak
// Creation Date:   May 12, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen for the popup when creating a new map

using Gum.Forms.Controls;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class NewMapScreen
    {
        /// <summary>
        /// Sets up screen on creation
        /// </summary>
        partial void CustomInitialize()
        {
            // Configure max # of chars
            MapName.MaxLength = Game1.MAX_LENGTH;
            Author.MaxLength = Game1.MAX_LENGTH;

            // Create new map
            CreateBtn.Click += (_, _) =>
            {
                // Play sfx
                SoundManager.PlayClick();

                // Trim inputted text
                MapName.Text = MapName.Text.Trim();
                Author.Text = Author.Text.Trim();

                // Only do something if both required textboxes are filled out
                if (MapName.Text != "" && Author.Text != "")
                {
                    // Add map to map list and save
                    Game1.maps.Add(new Map(MapName.Text, Author.Text));
                    Game1.SaveMaps();

                    // Close popup and refresh map select screen
                    GumService.Default.Root.Children.Clear();
                    new MapSelectScreen().AddToRoot();
                }
            };

            // Close popup
            CloseBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();
                this.RemoveFromRoot();
            };
        }
    }
}
