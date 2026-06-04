using Gum.Forms.Controls;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class NewMapScreen
    {
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
