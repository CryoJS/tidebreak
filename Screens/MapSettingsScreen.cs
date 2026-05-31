using System;
using System.Linq;
using Gum.Forms;
using Gum.Forms.Controls;
using MonoGame.Extended.Content.Tiled;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class MapSettingsScreen
    {
        partial void CustomInitialize()
        {
            // Ensure description has text wrapping and max length
            InputDesc.TextWrapping = TextWrapping.Wrap;
            InputDesc.MaxLength = Game1.MAX_LENGTH_LONG;

            // Ensure difficulty, drown speed, and flood speed are given valid floats
            InputDifficulty.PreviewTextInput += Game1.FloatOnlyHandler;
            InputDrownSpeed.PreviewTextInput += Game1.FloatOnlyHandler;
            InputFloodSpeed.PreviewTextInput += Game1.FloatOnlyHandler;

            // Ensure size x and y only recieves digits (to be an integer)
            InputSizeX.PreviewTextInput += Game1.IntegerOnlyHandler;
            InputSizeY.PreviewTextInput += Game1.IntegerOnlyHandler;

            // Store all float inputs
            float difficulty = Map.EMPTY;
            int sizeX = Map.EMPTY;
            int sizeY = Map.EMPTY;
            float drownSpeed = Map.EMPTY;
            float floodSpeed = Map.EMPTY;

            // If user presses save, change settings
            SaveBtn.Click += (_, _) =>
            {
                // Trim all inputs
                InputName.Text = InputName.Text.Trim();
                InputAuthor.Text = InputAuthor.Text.Trim();
                InputDesc.Text = InputDesc.Text.Trim();

                // Save them if they are valid
                if (InputName.Text != "") Game1.currentMap.Name = InputName.Text;
                if (InputAuthor.Text != "") Game1.currentMap.Author = InputAuthor.Text;
                if (InputDesc.Text != "") Game1.currentMap.Description = InputDesc.Text;

                // Convert floats if possible
                if (InputDifficulty.Text != "") difficulty = (float)Math.Round(Convert.ToDouble(InputDifficulty.Text), 1);
                if (InputSizeX.Text != "") sizeX = Convert.ToInt32(InputSizeX.Text);
                if (InputSizeY.Text != "") sizeY = Convert.ToInt32(InputSizeY.Text);
                if (InputDrownSpeed.Text != "") drownSpeed = Convert.ToSingle(InputDrownSpeed.Text);
                if (InputFloodSpeed.Text != "") floodSpeed = Convert.ToSingle(InputFloodSpeed.Text);

                // Check if floats are valid and save them if so
                if (Map.MIN_DIFF <= difficulty && difficulty < Map.MAX_EXC_DIFF) Game1.currentMap.Difficulty = difficulty;
                if (sizeX > 0) Game1.currentMap.SizeX = Math.Min(sizeX, Map.MAX_SIZE);
                if (sizeY > 0) Game1.currentMap.SizeY = Math.Min(sizeY, Map.MAX_SIZE);
                if (drownSpeed >= 0) Game1.currentMap.DrownSpeed = drownSpeed;
                if (floodSpeed >= 0) Game1.currentMap.FloodSpeed = floodSpeed;

                // If size is changed, copy over new map
                if (sizeX > 0 || sizeY > 0)
                {
                    // Temp store old tiles
                    Tile[,] oldTiles = Game1.currentMap.Tiles;
                    Tile[,] oldBgTiles = Game1.currentMap.BgTiles;

                    // Resize current tiles
                    Game1.currentMap.Tiles = new Tile[Game1.currentMap.SizeX, Game1.currentMap.SizeY];
                    Game1.currentMap.BgTiles = new Tile[Game1.currentMap.SizeX, Game1.currentMap.SizeY];

                    // Loop through all tiles, if doesn't exist, set as empty tile (otherwise copy over)
                    for (int x = 0; x < Game1.currentMap.SizeX; x++)
                    {
                        for (int y = 0; y < Game1.currentMap.SizeY; y++)
                        {
                            // Try to copy over, if failed (index out of range) just assign empty tile
                            try
                            {
                                Game1.currentMap.Tiles[x, y] = oldTiles[x, y];
                                Game1.currentMap.BgTiles[x, y] = oldBgTiles[x, y];
                            }
                            catch
                            {
                                Game1.currentMap.Tiles[x, y] = new Tile(x, y);
                                Game1.currentMap.BgTiles[x, y] = new Tile(x, y);
                            }
                        }
                    }
                }

                // Refresh map selection screen
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };

            //  If user presses edit, let them edit the map tiles
            EditBtn.Click += (_, _) =>
            {
                // Load map into editor
                Game1.mapEditor.Load(Game1.currentMap);

                // Update gamestate
                Game1.gameState = Game1.EDIT_MAP;

                // Change screen
                Game1.editScreen = new EditScreen();
                GumService.Default.Root.Children.Clear();
                Game1.editScreen.AddToRoot();
            };

            // If user presses exit, close popup
            CloseBtn.Click += (_, _) =>
            {
                this.RemoveFromRoot();
            };
        }
    }
}
