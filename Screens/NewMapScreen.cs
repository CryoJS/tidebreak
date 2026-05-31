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
                // Trim inputted text
                MapName.Text = MapName.Text.Trim();
                Author.Text = Author.Text.Trim();

                // Only do something if both required textboxes are filled out
                if (MapName.Text != "" && Author.Text != "")
                {
                    // Add map to map list and close popup
                    Game1.maps.Add(new Map(MapName.Text, Author.Text));

                    // Refresh map select screen
                    GumService.Default.Root.Children.Clear();
                    new MapSelectScreen().AddToRoot();
                }
            };

            // Close popup
            CloseBtn.Click += (_, _) =>
            {
                this.RemoveFromRoot();
            };
        }
    }
}
