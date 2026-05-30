using System;
using System.Linq;
using Gum.Forms.Controls;
using MonoGameGum;
using MonoGameGum.ExtensionMethods;
using Tidebreak.Components;

namespace Tidebreak.Screens
{
    partial class EditScreen
    {
        // Store sizings and speeds
        private const int DEFAULT_SCROLL_SPEED = 80;
        private const int TILE_SIZE = 128;
        private const int TILE_OFFSET = 20;

        // Store tile UI elements
        internal static TileUI[] platforms = new TileUI[Tile.PLATFORM_TYPE_AMOUNT];
        internal static TileUI[] decorative = new TileUI[Tile.DECORATIVE_TYPE_AMOUNT];
        internal static TileUI[] functional = new TileUI[Tile.FUNCTIONAL_TYPE_AMOUNT];

        // Store selected tile and current used bar
        public static TileUI[] Bar { get; private set; } = null;
        private static int selectedTileUI = Tile.NULL;

        partial void CustomInitialize()
        {
            // Configure tile viewer
            TileList.InnerPanel.ChildrenLayout = Gum.Managers.ChildrenLayout.LeftToRightStack;
            TileList.InnerPanel.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren;
            TileList.HorizontalScrollBarInstance.SmallChange = DEFAULT_SCROLL_SPEED;

            // Add all platform tiles
            platforms[0] = CreateTileUI(Tile.EMPTY, platforms);

            for (int type = 1; type < Tile.PLATFORM_TYPE_AMOUNT; type++)
            {
                platforms[type] = CreateTileUI(type, platforms);
            }

            // Add all decorative tiles
            for (int type = 0; type < Tile.DECORATIVE_TYPE_AMOUNT; type++)
            {
                decorative[type] = CreateTileUI(Tile.LADDER + type, decorative);
            }

            // Add all functional tiles
            functional[0] = CreateTileUI(Tile.WATER, functional);
            functional[1] = CreateTileUI(Tile.FLOOD, functional);
            functional[2] = CreateTileUI(Tile.BARRIER, functional);
            functional[3] = CreateTileUI(Tile.START, functional);
            functional[4] = CreateTileUI(Tile.END, functional);
            functional[5] = CreateTileUI(Tile.BUTTON, functional);
            functional[6] = CreateTileUI(Tile.WALL_JUMP, functional);
            functional[7] = CreateTileUI(Tile.ZIPLINE, functional);

            // Load platform tiles
            LoadTileBar(platforms);

            // Add platform bar logic
            PlatformBtn.Click += (_, _) =>
            {
                LoadTileBar(platforms);
            };

            // Add decorative bar logic
            DecorativeBtn.Click += (_, _) =>
            {
                LoadTileBar(decorative);
            };

            // Add functional bar logic
            FunctionalBtn.Click += (_, _) =>
            {
                LoadTileBar(functional);
            };

            // Add unselect logic
            UnselectBtn.Click += (_, _) =>
            {
                // Remove previously selected tile if it exists
                if (Bar != null && selectedTileUI != Tile.NULL) Bar[selectedTileUI].Selection.Visible = false;

                // Change to no selection
                Bar = null;
                selectedTileUI = Tile.NULL;

                // Set map editor selected tile to null
                MapEditor.SelectedTile = Tile.NULL;
            };

            // Add edit BG toggle logic
            BgBtn.Click += (_, _) =>
            {
                // Toggle background visibility
                Game1.mapEditor.EditBg = (bool)BgBtn.IsChecked;
            };

            // Add grid toggle logic
            GridBtn.Click += (_, _) =>
            {
                // Toggle grid visibility
                Game1.mapEditor.DisplayGrid = (bool)GridBtn.IsChecked;
            };

            // Save map if there are changes
            SaveBtn.Click += (_, _) =>
            {
                Game1.mapEditor.Save();
            };

            // When clicking undo button, undo
            UndoBtn.Click += (_, _) => Game1.mapEditor.Undo();

            // When clicking redo button, redo
            RedoBtn.Click += (_, _) => Game1.mapEditor.Redo();

            // Stop editing popup when pressing exit
            CloseBtn.Click += (_, _) =>
            {
                // If there are not changes, exit, otherwise prompt to save
                if (Game1.mapEditor.UndoStack.IsEmpty())
                {
                    MapSelectScreen newScreen = new MapSelectScreen();
                    GumService.Default.Root.Children.Clear();
                    newScreen.AddToRoot();
                }
                else
                {
                    StopEditScreen newScreen = new StopEditScreen();
                    newScreen.AddToRoot();
                }
            };
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
                // Remove previously selected tile if it exists
                if (Bar != null && selectedTileUI != Tile.NULL) Bar[selectedTileUI].Selection.Visible = false;
                
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
            var children = TileList.InnerPanelInstance.Children.ToArray(); // REVIEW can i do one line for loop, also can i use var?
            foreach (var child in children) child.Parent = null;

            // Load tiles into bar
            foreach (TileUI tile in tiles) TileList.AddChild(tile);
        }
    }
}