using System;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
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
                row.DifficultyText.Color = Map.diffColors[(int)map.difficulty];

                // Add button click behaviour: play the map
                row.PlayBtn.Click += (_, _) =>
                {
                    Game1.PlayMap(map);
                };

                // Add button click behaviour: look at map details, select it and open popup
                row.DetailsBtn.Click += (_, _) =>
                {
                    Game1.selectedMap = map;

                    MapDetailsScreen newScreen = new MapDetailsScreen();
                    newScreen.AddToRoot();
                };

                // Edit the map only if map is not locked
                if (!map.locked)
                {
                    // Add edit map button click behaviour
                    row.EditBtn.Click += (_, _) =>
                    {
                        // Change gamestate (and map)
                        Game1.currentMap = map;
                        Game1.gameState = Game1.EDIT_MAP;
                        
                        // Change screen
                        EditScreen newScreen = new EditScreen();
                        GumService.Default.Root.Children.Clear();
                        newScreen.AddToRoot();
                    };
                }
                else
                {
                    row.EditBtn.IsVisible = false;
                }

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
