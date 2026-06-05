using System;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.ExtensionMethods;
using Tidebreak.Components;

namespace Tidebreak.Screens
{
    partial class EditScreen
    {
        // Store sizings and speeds
        private const int DEFAULT_SCROLL_SPEED = 1;
        private const int TILE_SIZE = 90;
        private const int TILE_OFFSET = 20;

        // Store tile UI elements
        internal static TileUI[] functional = new TileUI[(int)Tile.Func.EndIndex - Tile.FUNC_START];
        internal static TileUI[] platforms = new TileUI[(int)Tile.Plat.EndIndex - Tile.PLAT_START + 1];     // Also includes empty for convenience
        internal static TileUI[] decorative = new TileUI[(int)Tile.Decor.EndIndex - Tile.DECOR_START + 1];  // Also includes empty for convenience
        internal static TileUI[] colors = new TileUI[(int)Tile.Clr.EndIndex - Tile.CLR_START + 1];          // Also includes empty for convenience

        // Store selected tile and current used bar
        public static TileUI[] Bar { get; private set; } = null;
        private static int selectedTileUI = (int)Tile.Func.Null;

        partial void CustomInitialize()
        {
            // Configure tile viewer
            TileList.InnerPanel.ChildrenLayout = Gum.Managers.ChildrenLayout.LeftToRightStack;
            TileList.InnerPanel.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
            TileList.HorizontalScrollBarInstance.SmallChange = DEFAULT_SCROLL_SPEED;

            // Add all functional tiles
            for (int type = (int)Tile.FUNC_START; type < (int)Tile.Func.EndIndex; type++)
            {
                // Ensure pressed button variant is ignored and proper indexing skip is accounted for
                if (type != (int)Tile.Func.PressedButton)
                {
                    functional[type - (int)Tile.FUNC_START - (type < (int)Tile.Func.PressedButton ? 0 : 1)] = CreateTileUI(type, functional);
                }
            }
            
            // Add zipline
            functional[(int)Tile.Func.EndIndex - 1] = CreateTileUI(Tile.ZIPLINE, functional);

            // Add empty to all bars
            platforms[0] = CreateTileUI((int)Tile.Func.Empty, platforms);
            decorative[0] = CreateTileUI((int)Tile.Func.Empty, decorative);
            colors[0] = CreateTileUI((int)Tile.Func.Empty, colors);

            // Add all platform tiles
            for (int type = Tile.PLAT_START; type < (int)Tile.Plat.EndIndex; type++)
            {
                platforms[type - Tile.PLAT_START + 1] = CreateTileUI(type, platforms);
            }

            // Add all decorative tiles
            for (int type = Tile.DECOR_START; type < (int)Tile.Decor.EndIndex; type++)
            {
                decorative[type - Tile.DECOR_START + 1] = CreateTileUI(type, decorative);
            }
            
            // Add all color tiles
            for (int type = Tile.CLR_START; type < (int)Tile.Clr.EndIndex; type++)
            {
                colors[type - Tile.CLR_START + 1] = CreateTileUI(type, colors);
            }

            // Load functional tiles as default open
            LoadTileBar(functional);

            // Add functional bar logic
            FunctionalBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                LoadTileBar(functional);
            };

            // Add platform bar logic
            PlatformBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                LoadTileBar(platforms);
            };

            // Add decorative bar logic
            DecorativeBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                LoadTileBar(decorative);
            };

            // Add colors bar logic
            ColorsBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                LoadTileBar(colors);
            };

            // Ensure options are in sync
            FgBtn.IsChecked = Game1.mapEditor.ShowFg;
            BgBtn.IsChecked = Game1.mapEditor.ShowBg;

            // Add unselect logic
            UnselectBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();
                
                // Remove previously selected tile if it exists
                if (Bar != null && selectedTileUI != (int)Tile.Func.Null) Bar[selectedTileUI].Selection.Visible = false;

                // Change to no selection
                Bar = null;
                selectedTileUI = (int)Tile.Func.Null;

                // Set map editor selected tile to null
                MapEditor.SelectedTile = (int)Tile.Func.Null;
            };

            // Add show foreground toggle logic
            FgBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.mapEditor.ShowFg = (bool)FgBtn.IsChecked;
            };

            // Add show background toggle logic
            BgBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.mapEditor.ShowBg = (bool)BgBtn.IsChecked;
            };

            // Add edit background toggle logic
            EditBgBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.mapEditor.EditBg = (bool)EditBgBtn.IsChecked;
            };

            // Add grid toggle logic
            GridBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.mapEditor.DisplayGrid = (bool)GridBtn.IsChecked;
            };

            // Save map if there are changes
            SaveBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();

                Game1.mapEditor.Save();
                Game1.SaveMaps();
            };

            // When clicking undo button, undo
            UndoBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.mapEditor.Undo();
            };

            // When clicking redo button, redo
            RedoBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.mapEditor.Redo();
            };

            // Stop editing popup when pressing exit
            CloseBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // If there are not changes, exit, otherwise prompt to save
                if (SaveBtn.IsEnabled)
                {
                    new StopEditScreen().AddToRoot();
                }
                else
                {
                    GumService.Default.Root.Children.Clear();
                    Game1.gameState = Game1.SELECT_MAP;
                    new MapSelectScreen().AddToRoot();
                }
            };
        }

        public void Update(MouseState mouse, MouseState prevMouse)
        {
            // Update scroll value by scroll amount
            TileList.HorizontalScrollBarValue -= (mouse.ScrollWheelValue - prevMouse.ScrollWheelValue) * DEFAULT_SCROLL_SPEED;
        }

        private TileUI CreateTileUI(int tileType, TileUI[] tiles)
        {
            // Create new tile
            TileUI tile = new TileUI();

            // Give the tile a texture
            tile.SpriteInstance.Texture = Tile.GetTexture(tileType);

            // Assign tile type
            tile.Type = tileType;

            // Assign tile visual properties
            tile.Width = TILE_SIZE;
            tile.Height = TILE_SIZE;
            tile.X = TILE_OFFSET;
            tile.Y = TILE_OFFSET;
            tile.Selection.Visible = false;

            // Add tile select logic
            tile.Btn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // Remove previously selected tile if it exists
                if (Bar != null && selectedTileUI != (int)Tile.Func.Null) Bar[selectedTileUI].Selection.Visible = false;
                
                // Change to new selection
                Bar = tiles;
                selectedTileUI = tiles.IndexOf(tile);

                // Select new selection
                Bar[selectedTileUI].Selection.Visible = true;
                MapEditor.SelectedTile = tile.Type;
            };
            
            // Return the tile
            return tile;
        }

        private void LoadTileBar(TileUI[] tiles)
        {
            // Remove all children
            TileList.InnerPanelInstance.Children.Clear();

            // Rendering issue, scroll bar causes game area to be black
            TileList.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

            // Load tiles into bar
            foreach (TileUI tile in tiles) TileList.AddChild(tile);
        }
    }
}