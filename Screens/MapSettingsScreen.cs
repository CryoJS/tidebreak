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

            // Ensure difficulty is a valid float
            InputDifficulty.PreviewTextInput += (sender, args) =>
            {
                // Store current input and calculate new input 
                TextBox textBox = (TextBox)sender;
                string newText = textBox.Text.Insert(textBox.CaretIndex, args.Text);
                
                // Check if new input is valid using try parse
                args.Handled = !float.TryParse(newText, out _);
            };

            // Ensure drown speed is a valid float
            InputDrownSpeed.PreviewTextInput += (sender, args) =>
            {
                // Store current input and calculate new input 
                TextBox textBox = (TextBox)sender;
                string newText = textBox.Text.Insert(textBox.CaretIndex, args.Text);
                
                // Check if new input is valid using try parse
                args.Handled = !float.TryParse(newText, out _);
            };

            // Ensure flood speed is a valid float
            InputFloodSpeed.PreviewTextInput += (sender, args) =>
            {
                // Store current input and calculate new input 
                TextBox textBox = (TextBox)sender;
                string newText = textBox.Text.Insert(textBox.CaretIndex, args.Text);
                
                // Check if new input is valid using try parse
                args.Handled = !float.TryParse(newText, out _);
            };

            // Ensure size x only recieves digits (to be an integer)
            InputSizeX.PreviewTextInput += (sender, args) =>
            {
                if (args.Text.Any(item => !char.IsDigit(item)))
                {
                    args.Handled = true;
                }
            };

            // Ensure size x only recieves digits (to be an integer)
            InputSizeY.PreviewTextInput += (sender, args) =>
            {
                if (args.Text.Any(item => !char.IsDigit(item)))
                {
                    args.Handled = true;
                }
            };

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
                if (InputName.Text != "") Game1.currentMap.name = InputName.Text;
                if (InputAuthor.Text != "") Game1.currentMap.author = InputAuthor.Text;
                if (InputDesc.Text != "") Game1.currentMap.description = InputDesc.Text;

                // Convert floats if possible
                if (InputDifficulty.Text != "") difficulty = (float)Math.Round(Convert.ToDouble(InputDifficulty.Text), 1);
                if (InputSizeX.Text != "") sizeX = Convert.ToInt32(InputSizeX.Text);
                if (InputSizeY.Text != "") sizeY = Convert.ToInt32(InputSizeY.Text);
                if (InputDrownSpeed.Text != "") drownSpeed = Convert.ToSingle(InputDrownSpeed.Text);
                if (InputFloodSpeed.Text != "") floodSpeed = Convert.ToSingle(InputFloodSpeed.Text);

                // Check if floats are valid and save them if so
                if (Map.MIN_DIFF <= difficulty && difficulty < Map.MAX_EXC_DIFF) Game1.currentMap.difficulty = difficulty;
                if (sizeX > 0) Game1.currentMap.sizeX = Math.Min(sizeX, Map.MAX_SIZE);
                if (sizeY > 0) Game1.currentMap.sizeY = Math.Min(sizeY, Map.MAX_SIZE);
                if (drownSpeed >= 0) Game1.currentMap.drownSpeed = drownSpeed;
                if (floodSpeed >= 0) Game1.currentMap.floodSpeed = floodSpeed;

                // If size is changed, copy over new map
                if (sizeX > 0 || sizeY > 0)
                {
                    // Temp store old tiles
                    Tile[,] oldTiles = Game1.currentMap.tiles;
                    Tile[,] oldBgTiles = Game1.currentMap.bgTiles;
                    bool[,] oldFloodTiles = Game1.currentMap.floodTiles;

                    // Resize current tiles
                    Game1.currentMap.tiles = new Tile[Game1.currentMap.sizeX, Game1.currentMap.sizeY];
                    Game1.currentMap.bgTiles = new Tile[Game1.currentMap.sizeX, Game1.currentMap.sizeY];
                    Game1.currentMap.floodTiles = new bool[Game1.currentMap.sizeX, Game1.currentMap.sizeY];

                    // Loop through all tiles, if doesn't exist, set as empty tile (otherwise copy over)
                    for (int x = 0; x < Game1.currentMap.sizeX; x++)
                    {
                        for (int y = 0; y < Game1.currentMap.sizeY; y++)
                        {
                            // Try to copy over, if failed (index out of range) just assign empty tile
                            try
                            {
                                Game1.currentMap.tiles[x, y] = oldTiles[x, y];
                                Game1.currentMap.bgTiles[x, y] = oldBgTiles[x, y];
                                Game1.currentMap.floodTiles[x, y] = oldFloodTiles[x, y];
                            }
                            catch
                            {
                                Game1.currentMap.tiles[x, y] = new Tile(x, y);
                                Game1.currentMap.bgTiles[x, y] = new Tile(x, y);
                                Game1.currentMap.floodTiles[x, y] = false;
                            }
                        }
                    }
                }

                // Refresh map selection screen
                MapSelectScreen newScreen = new MapSelectScreen();
                GumService.Default.Root.Children.Clear();
                newScreen.AddToRoot();
            };

            //  If user presses edit, let them edit the map tiles
            EditBtn.Click += (_, _) =>
            {
                // Update gamestate
                Game1.gameState = Game1.EDIT_MAP;

                // Change screen
                EditScreen newScreen = new EditScreen();
                GumService.Default.Root.Children.Clear();
                newScreen.AddToRoot();
            };

            // If user presses exit, close popup
            CloseBtn.Click += (_, _) =>
            {
                this.RemoveFromRoot();
            };
        }
    }
}
