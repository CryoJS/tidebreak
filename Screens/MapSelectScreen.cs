using Gum.Forms.Controls;
using MonoGameGum;
using Tidebreak.Components;

namespace Tidebreak.Screens
{
    partial class MapSelectScreen
    {
        // Store default scroll speed
        private const int DEFAULT_SCROLL_SPEED = 50;

        partial void CustomInitialize()
        {
            // Increase scroll speed (its sooo slow by default)
            MapList.VerticalScrollBarInstance.SmallChange = DEFAULT_SCROLL_SPEED;

            // Send player back to menu
            ReturnBtn.Click += (_, _) =>
            {
                // Change screen
                var newScreen = new TitleScreen();
                this.RemoveFromRoot();
                newScreen.AddToRoot();

                // Change gamestate
                Game1.gameState = Game1.MENU;
            };

            // Load all maps into map UI
            foreach (var map in Game1.maps)
            {
                // Create new map entry
                var row = new MapRow();

                // Update text in each row with the map info
                row.TitleText.Text = map.name;
                row.AuthorText.Text = map.author;
                row.DifficultyText.Text = map.difficulty.ToString("F1");

                // Add button click behaviour: play the map
                row.PlayBtn.Click += (_, _) =>
                {
                    // Change screen
                    var newScreen = new PlayScreen();
                    this.RemoveFromRoot();
                    newScreen.AddToRoot();

                    // Change gamestate (and map)
                    Game1.currentMap = Game1.maps.IndexOf(map);
                    Game1.gameState = Game1.PLAY_MAP;

                    // Load map
                    Game1.maps[Game1.currentMap].Start(Game1.player);
                };

                // Add button click behaviour: edit the map
                row.EditBtn.Click += (_, _) =>
                {
                    // Change screen
                    var newScreen = new EditScreen();
                    this.RemoveFromRoot();
                    newScreen.AddToRoot();

                    // Change gamestate (and map)
                    Game1.currentMap = Game1.maps.IndexOf(map);
                    Game1.gameState = Game1.EDIT_MAP;
                };

                // Add finalized map row into list of maps
                MapList.InnerPanelInstance.Children.Add(row.Visual);
            }
        }
    }
}
