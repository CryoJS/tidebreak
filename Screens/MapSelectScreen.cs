using MonoGameGum;
using Tidebreak.Components;

namespace Tidebreak.Screens
{
    partial class MapSelectScreen
    {
        // Store default scroll speed
        private const int DEFAULT_SCROLL_SPEED = 50;

        partial void CustomInitialize() // REVIEW do i need to add documentation for custum initialize functions that are used for ui screens (and other functions i make in here)
        {
            // Play lobby music
            SoundManager.PlayLobbyMusic();
            
            // Increase scroll speed (its sooo slow by default)
            MapList.VerticalScrollBarInstance.SmallChange = DEFAULT_SCROLL_SPEED;

            // Load all maps into map UI
            foreach (Map map in Game1.maps)
            {
                // Create new map row entry
                MapRow row = new MapRow();

                // Update text in each row with the map info
                row.TitleText.Text = map.Name;
                row.AuthorText.Text = map.Author;
                row.DifficultyText.Text = map.Difficulty.ToString("F1");
                row.DifficultyText.Color = Map.diffColors[(int)map.Difficulty];

                // Add button click behaviour: play the map
                row.PlayBtn.Click += (_, _) =>
                {
                    SoundManager.PlayClick();
                    Game1.PlayMap(map);
                };

                // Add button click behaviour: look at map details, select it and open popup
                row.DetailsBtn.Click += (_, _) =>
                {
                    SoundManager.PlayClick();
                    Game1.currentMap = map;
                    new MapDetailsScreen().AddToRoot();
                };

                // Edit the map only if map is not locked
                if (!map.Locked)
                {
                    // Add edit map button click behaviour
                    row.EditBtn.Click += (_, _) =>
                    {
                        SoundManager.PlayClick();

                        // Change gamestate (and map)
                        Game1.currentMap = map;
                        
                        // Add map settings popup
                        new MapSettingsScreen().AddToRoot();
                    };
                }
                else
                {
                    row.EditBtn.IsVisible = false;
                }

                // Add finalized map row into list of maps
                MapList.AddChild(row.Visual);
            }

            // Create new map
            NewMapBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                new NewMapScreen().AddToRoot();
            };

            // Open map sort menu
            SortBtn.Click  += (_, _) =>
            {
                SoundManager.PlayClick();
                new MapSortScreen().AddToRoot();
            };

            // Send player back to menu
            ReturnBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.ReturnToMenu();
            };
        }
    }
}
