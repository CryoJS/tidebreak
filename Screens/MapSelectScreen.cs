using Gum.Forms.Controls;
using MonoGameGum;
using Tidebreak.Components;

namespace Tidebreak.Screens
{
    partial class MapSelectScreen
    {
        // Store default scroll speed
        private const int DEFAULT_SCROLL_SPEED = 50;

        partial void CustomInitialize() // REVIEW do i need to add documentation for custum initialize functions that are used for ui screens
        {
            // Increase scroll speed (its sooo slow by default)
            MapList.VerticalScrollBarInstance.SmallChange = DEFAULT_SCROLL_SPEED;

            // Load all maps into map UI
            foreach (Map map in Game1.maps)
            {
                // Create new map row entry
                MapRow row = new MapRow();

                // Update text in each row with the map info
                row.TitleText.Text = map.name;
                row.AuthorText.Text = map.author;
                row.DifficultyText.Text = map.difficulty.ToString("F1");

                // Add button click behaviour: play the map
                row.PlayBtn.Click += (_, _) =>
                {
                    // Change screen
                    PlayScreen newScreen = new PlayScreen();
                    GumService.Default.Root.Children.Clear();
                    newScreen.AddToRoot();

                    // Change gamestate (and map)
                    Game1.currentMap = Game1.maps.IndexOf(map);
                    Game1.gameState = Game1.PLAY_MAP;

                    // Load map
                    Game1.maps[Game1.currentMap].Start(Game1.player, Game1.camera);
                };

                // Add button click behaviour: edit the map
                row.EditBtn.Click += (_, _) =>
                {
                    // Change screen
                    EditScreen newScreen = new EditScreen();
                    GumService.Default.Root.Children.Clear();
                    newScreen.AddToRoot();

                    // Change gamestate (and map)
                    Game1.currentMap = Game1.maps.IndexOf(map);
                    Game1.gameState = Game1.EDIT_MAP;
                };

                // Add finalized map row into list of maps
                MapList.InnerPanelInstance.Children.Add(row.Visual);
            }

            // Send player back to menu
            ReturnBtn.Click += (_, _) =>
            {
                Game1.ReturnToMenu();
            };
        }
    }
}
