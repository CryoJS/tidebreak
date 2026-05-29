using System;
using System.Collections.Generic;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using Tidebreak;
using Tidebreak.Screens;

class MapEditor
{
    // Store visual constants for the editor
    private const float SELECTION_TRANSPARENCY = 0.5f;
    private const float CAMERA_MOVE_BORDER = 500f;

    // Store move and zoom speed factor constant
    private const float MOVE_SPEED = 2000;
    private const float MAX_MOVE_SPEED = 6000;
    private const float ZOOM_SPEED = 0.2f;

    // Store camera zoom constants
    private const float MIN_ZOOM = 0.1f;
    private const float MAX_ZOOM = 2f;

    // Store grid lines thickness
    private const float GRID_THICKNESS = 7f;
    private readonly Color gridColor = Color.Black;

    // Store empty selected tile constant
    private readonly Point NO_TILE_SELECTED = new Point(-1, -1);

    // Store the edited map
    Map map;

    // Store the changed tiles of the edited map
    public Tile[,] Tiles { get; private set; }
    public Tile[,] BgTiles { get; private set; }
    private int[,] floodTiles;

    // Store the changed buttons and ziplines of the edited map
    BSTree<Button> buttons;
    List<Zipline> ziplines;

    // Store the stacks for undo and redo
    public Stack<List<TileEdit>> UndoStack { get; private set; }
    public Stack<List<TileEdit>> RedoStack { get; private set; }

    // Store the camera and desired camera position and zoom to view the map
    public Camera Camera { get; private set; }
    private Vector2 pos;
    private float zoom;

    // Store variables for display options
    public bool DisplayGrid { get; set; }
    public bool EditBg { get; set; }

    private GameLine[] verLines;
    private GameLine[] horLines;

    // Store selected tile to place
    public static int SelectedTile { get; set; } = Tile.NULL;
    private int newType;

    public static Point StartSelected { get; set; }
    public bool IsEditing { get; set; }

    // Store needed info to ensure map works
    private int startCnt;

    public MapEditor() {}

    public void Load(Map map)
    {
        // Store given map
        this.map = map;

        // Reset undo and redo stacks
        UndoStack = new Stack<List<TileEdit>>();
        RedoStack = new Stack<List<TileEdit>>();

        // Create space to store the changed map
        Tiles = new Tile[map.SizeX, map.SizeY];
        BgTiles = new Tile[map.SizeX, map.SizeY];
        floodTiles = new int[map.SizeX, map.SizeY];

        // Reset editing and start count
        IsEditing = true;
        startCnt = 0;

        // Copy over all tiles (by looping through them)
        for (int x = 0; x < map.SizeX; x++)
        {
            for (int y = 0; y < map.SizeY; y++)
            {
                Tiles[x, y] = map.Tiles[x, y].Copy();
                BgTiles[x, y] = map.BgTiles[x, y].Copy();
                floodTiles[x, y] = map.FloodTiles[x, y];

                // Keep track of start tile amount
                if (Tiles[x, y].Type == Tile.START) startCnt++;
            }
        }

        // Copy over BST of buttons
        buttons = map.Buttons.Copy();

        // Copy over list of ziplines
        ziplines = new(map.Ziplines);

        // Create/reset camera zoom and position with top left at origin
        Camera = new Camera(Game1._graphics.GraphicsDevice.Viewport);
        Camera.SetPos(new Vector2(Game1.ScreenWidth, Game1.ScreenHeight) / 2);

        // Store camera pos and zoom
        pos = Camera.GetPos();
        zoom = Camera.GetZoom();

        // Set grid lines default to not visible
        DisplayGrid = false;
        EditBg = false;

        // Add space to store gridlines
        verLines = new GameLine[map.SizeX + 1];
        horLines = new GameLine[map.SizeY + 1];

        // Create gridlines
        for (int x = 0; x <= map.SizeX; x++) verLines[x] = new GameLine(Game1._graphics.GraphicsDevice, new Vector2(x * Game1.TILE_SIZE * Game1.PIXEL_SCALE, 0), new Vector2(x * Game1.TILE_SIZE * Game1.PIXEL_SCALE, map.SizeY * Game1.TILE_SIZE * Game1.PIXEL_SCALE), GRID_THICKNESS);
        for (int y = 0; y <= map.SizeY; y++) horLines[y] = new GameLine(Game1._graphics.GraphicsDevice, new Vector2(0, y * Game1.TILE_SIZE * Game1.PIXEL_SCALE), new Vector2(map.SizeX * Game1.TILE_SIZE * Game1.PIXEL_SCALE, y * Game1.TILE_SIZE * Game1.PIXEL_SCALE), GRID_THICKNESS);

        // Reset selection
        StartSelected = NO_TILE_SELECTED;
    }

    public void Update(GameTime gameTime, KeyboardState kb, KeyboardState prevKb, MouseState mouse, MouseState prevMouse)
    {
        // Move camera
        Move(gameTime, kb, mouse, prevMouse);

        // Clamp pos and zoom
        pos = Vector2.Clamp(pos, new Vector2(-1, -1) * CAMERA_MOVE_BORDER, new Vector2(map.SizeX, map.SizeY) * Game1.TILE_SIZE * Game1.PIXEL_SCALE + new Vector2(1, 1) * CAMERA_MOVE_BORDER);
        zoom = Math.Clamp(zoom, MIN_ZOOM, MAX_ZOOM);

        // Update camera
        Camera.Update(gameTime, pos);
        Camera.ZoomUpdate(gameTime, zoom);

        // Only check for tile placing if mouse isn't on the bar
        if (IsTileClickable(mouse.Position.ToVector2()))
        {
            // Check tile place start
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                // Get tile coordinates of mouse position
                (int tileX, int tileY) = Game1.CalcTile(Camera.ScreenToWorld(mouse.Position.ToVector2()));

                // Check if tile coordinates are within bounds of map
                if (tileX >= 0 && tileX < map.SizeX && tileY >= 0 && tileY < map.SizeY)
                {
                    StartSelected = new Point(tileX, tileY);
                }
            }

            // Check tile place end
            if (StartSelected != NO_TILE_SELECTED && mouse.LeftButton != ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Pressed)
            {
                // Get tile coordinates of mouse position
                (int tileX, int tileY) = Game1.CalcTile(Camera.ScreenToWorld(mouse.Position.ToVector2()));

                // Check if tile coordinates are within bounds of map
                if (tileX >= 0 && tileX < map.SizeX && tileY >= 0 && tileY < map.SizeY)
                {
                    // If no tile is selected, check if they are editing button, otherwise place tiles
                    if (SelectedTile == Tile.NULL)
                    {
                        // Check if they selected a button
                        if (Tile.GetType(Tiles[StartSelected.X, StartSelected.Y].Type, true) == Tile.BUTTON)
                        {
                            // If they are editing a button, open the edit button popup for that button, otherwise start editing that button
                            EditButtonScreen newScreen = new EditButtonScreen();
                            newScreen.AddToRoot();

                            // Let the editor know popup is open (so other actions can be blocked)
                            IsEditing = false;
                        }
                    }
                    else
                    {
                        // Store new tile
                        newType = SelectedTile;

                        // If button, calculate new type
                        if (SelectedTile == Tile.BUTTON) newType = Button.PriorityToType(buttons.GetRightmost().Priority + 1);

                        // Place selected tile at the tile coordinates
                        PlaceTiles(new Point(Math.Min(StartSelected.X, tileX), Math.Min(StartSelected.Y, tileY)), new Point(Math.Max(StartSelected.X, tileX), Math.Max(StartSelected.Y, tileY)));
                    }
                }
            }

            // If player let go, reset selection (only if editing)
            if (IsEditing && mouse.LeftButton != ButtonState.Pressed)
            {
                StartSelected = NO_TILE_SELECTED;
            }
        }

        // Check redo input and undo input
        if (kb.IsKeyDown(Keys.LeftControl) && kb.IsKeyDown(Keys.Z) && !prevKb.IsKeyDown(Keys.Z))
        {
            // If they are pressing shift, redo, otherwise undo
            if (kb.IsKeyDown(Keys.LeftShift)) Redo();
            else Undo();
        }

        // Update undo and redo buttons state
        Game1.editScreen.UndoBtn.IsEnabled = !UndoStack.IsEmpty();
        Game1.editScreen.RedoBtn.IsEnabled = !RedoStack.IsEmpty();

        // Ensure saving is only possible when there is at least 1 start tile
        Game1.editScreen.SaveBtn.IsEnabled = Game1.editScreen.SaveBtn.IsEnabled = startCnt > 0;
    }

    public void Draw(SpriteBatch spriteBatch, MouseState mouse)
    {
        // Draw each tile, background tile first, 
        for (int x = 0; x < map.SizeX; x++)
        {
            for (int y = 0; y < map.SizeY; y++)
            {
                // Draw background tile first
                BgTiles[x, y].Draw(spriteBatch, true);

                // Draw foreground tiles after
                if (!EditBg) Tiles[x, y].Draw(spriteBatch, true);
            }
        }

        // Draw all the ziplines if we are drawing the foreground
        if (!EditBg)
        {
            foreach (Zipline zipline in ziplines)
            {
                zipline.Draw(spriteBatch);
            }
        }

        // Draw tile selection box if hovering over a tile
        if (IsTileClickable(mouse.Position.ToVector2()))
        {
            // Store tile coordinates of mouse position
            (int tileX, int tileY) = Game1.CalcTile(Camera.ScreenToWorld(mouse.Position.ToVector2()));

            // Check if tile is within bounds of the map
            if (tileX >= 0 && tileX < map.SizeX && tileY >= 0 && tileY < map.SizeY)
            {
                // Draw all if selecting multiple, other just draw the tile being hovered over
                if (StartSelected == NO_TILE_SELECTED)
                {
                    spriteBatch.Draw(Tile.SelectImg, Tiles[tileX, tileY].Rec, Color.White * SELECTION_TRANSPARENCY);
                }
                else
                {
                    // Loop through all tiles in the selection and draw the selection highlight on them
                    for (int x = Math.Min(StartSelected.X, tileX); x <= Math.Max(StartSelected.X, tileX); x++)
                    {
                        for (int y = Math.Min(StartSelected.Y, tileY); y <= Math.Max(StartSelected.Y, tileY); y++)
                        {
                            spriteBatch.Draw(Tile.SelectImg, Tiles[x, y].Rec, Color.White * SELECTION_TRANSPARENCY);
                        }
                    }
                }
            }
        }

        // Draw map guidelines when toggled, if no grid, still draw borders
        if (DisplayGrid)
        {
            for (int x = 0; x <= map.SizeX; x++) verLines[x].Draw(spriteBatch, gridColor);
            for (int y = 0; y <= map.SizeY; y++) horLines[y].Draw(spriteBatch, gridColor);
        }
        else
        {
            verLines[0].Draw(spriteBatch, Color.White);
            verLines[map.SizeX].Draw(spriteBatch, Color.White);
            horLines[0].Draw(spriteBatch, Color.White);
            horLines[map.SizeY].Draw(spriteBatch, Color.White);
        }
    }

    private void Move(GameTime gameTime, KeyboardState kb, MouseState mouse, MouseState prevMouse)
    {
        // Calculate move amount
        float moveAmount = Math.Min(MAX_MOVE_SPEED, MOVE_SPEED / zoom) * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Move by zoom amount left, right, up, and down
        if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left)) pos.X -= moveAmount;
        if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right)) pos.X += moveAmount;
        if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up)) pos.Y -= moveAmount;
        if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down)) pos.Y += moveAmount;

        // Zoom in and zoom out
        zoom += (mouse.ScrollWheelValue - prevMouse.ScrollWheelValue) * ZOOM_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    private bool IsTileClickable(Vector2 mousePos)
    {
        return IsEditing
            && !Game1.editScreen.BarContainer.IsPointInside(mousePos.X, mousePos.Y)
            && !Game1.editScreen.TopLeftContainer.IsPointInside(mousePos.X, mousePos.Y)
            && !Game1.editScreen.TopRightContainer.IsPointInside(mousePos.X, mousePos.Y);
    }

    private void ExtraRemove(int type, int x, int y)
    {
        // Perform any needed updates for the removed tile
        switch (Tile.GetType(type, true))
        {
            case Tile.START:
                // Decrement start tile count
                startCnt--;
                break;

            case Tile.BUTTON:
                // Remove button from BST
                buttons.Delete(new Button(x, y, Button.TypeToPriority(type)));
                break;
        }
    }

    private void ExtraAdd(int type, int x, int y)
    {
        // Perform any needed updates for the added tile
        switch (Tile.GetType(type, true))
        {
            case Tile.START:
                // Increment start tile count
                startCnt++;
                break;

            case Tile.BUTTON:
                // Add button to BST
                buttons.Add(new Button(x, y, Button.TypeToPriority(type)));
                break;
        }
    }

    private void ChangeTile(int x, int y)
    {
        // Store tile and removed tile type and do any extra updates
        Tile curTile = EditBg ? BgTiles[x, y] : Tiles[x, y];
        ExtraRemove(curTile.Type, x, y);

        // Add tile edit to current stack action of changing all selected tiles
        UndoStack.Top().Add(new TileEdit(x, y, EditBg, curTile.Type, newType));

        // Replace tile and perform any needed updates
        curTile.Type = newType;
        ExtraAdd(newType, x, y);
    }

    private void PlaceTiles(Point start, Point end)
    {
        // If no selected tile, do nothing
        if (newType == Tile.NULL) return;

        // If the tile is a functional block, then do not place it if trying to be place on bg
        if (EditBg && EditScreen.Bar == EditScreen.functional) return;

        // Add new action
        AddNewAction();
    
        // Loop through each tile
        for (int x = start.X; x <= end.X; x++)
        {
            for (int y = start.Y; y <= end.Y; y++)
            {
                ChangeTile(x, y);
            }
        }
    }

    private void AddNewAction()
    {
        // Add new action to stack and clear redo stack
        UndoStack.Push(new List<TileEdit>());
        RedoStack.Clear();
    }

    public void Undo()
    {
        // If nothing to undo, do nothing
        if (UndoStack.IsEmpty()) return;

        // TODO
    }

    public void Redo()
    {
        // If nothing to redo, do nothing
        if (RedoStack.IsEmpty()) return;


    }

    public void ChangeButtonSettings(int newPriority)
    {
        // If priority is the same as any existing buttons, do nothing
        if (buttons.Find(new Button(0, 0, newPriority)) != null) return;

        // Ensure button changes are selected
        newType = Button.PriorityToType(newPriority);

        // Add new action and update the button
        AddNewAction();
        ChangeTile(StartSelected.X, StartSelected.Y);
    }

    public void Save()
    {
        // Save the changed tiles back to the map
        for (int x = 0; x < map.SizeX; x++)
        {
            for (int y = 0; y < map.SizeY; y++)
            {
                map.Tiles[x, y] = Tiles[x, y];
                map.BgTiles[x, y] = BgTiles[x, y];
                map.FloodTiles[x, y] = floodTiles[x, y];
            }
        }

        // Save the changed buttons back to the map
        map.Buttons = buttons.Copy();

        // Save the changed ziplines back to the map
        map.Ziplines = ziplines;
    }
}