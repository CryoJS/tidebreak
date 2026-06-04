using System;
using System.Collections.Generic;
using System.Linq;
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
    private const float ZOOM_SPEED = 1.2f;

    // Store camera zoom constants
    private const float MIN_ZOOM = 0.02f;
    private const float MAX_ZOOM = 2;

    // Store grid lines thickness
    private const float GRID_THICKNESS = 7f;
    private readonly Color gridColor = Color.White;

    // Store empty selected tile constant
    private readonly Point NO_TILE_SELECTED = new Point(-1, -1);

    // Store the edited map
    private Map map;

    // Store the changed tiles of the edited map
    public Tile[,] Tiles { get; private set; }
    public Tile[,] BgTiles { get; private set; }

    // Store the changed buttons and ziplines of the edited map
    private BSTree<Button> buttons;
    private List<Zipline> ziplines;

    // Store the stacks for undo and redo
    public Stack<Stack<TileEdit>> UndoStack { get; private set; }
    public Stack<Stack<TileEdit>> RedoStack { get; private set; }

    // Store the camera and desired camera position and zoom to view the map
    public Camera Camera { get; private set; }
    private Vector2 pos;
    private float zoom;

    // Store variables for display options
    public bool DisplayGrid { get; set; }
    public bool EditBg { get; set; }

    public bool ShowFg { get; set; }
    public bool ShowBg { get; set; }

    private GameLine[] verLines;
    private GameLine[] horLines;

    // Store selected tile to place
    public static int SelectedTile { get; set; } = Tile.NULL;
    private int newType;

    public static Point StartSelected { get; set; }
    public bool IsEditing { get; set; }

    // Store needed info to ensure map works
    private int startCnt;

    // Store needed save variables
    private bool unsaved;
    private bool ziplineEdited;

    public MapEditor() {}

    public void Load(Map map)
    {
        // Store given map
        this.map = map;

        // Reset undo and redo stacks
        UndoStack = new Stack<Stack<TileEdit>>();
        RedoStack = new Stack<Stack<TileEdit>>();

        // Create space to store the changed map
        Tiles = new Tile[map.SizeX, map.SizeY];
        BgTiles = new Tile[map.SizeX, map.SizeY];

        // Reset editing and start count
        IsEditing = true;
        startCnt = 0;

        // Map is saved already at the start, ziplines not touched
        unsaved = false;
        ziplineEdited = false;

        // Copy over all tiles (by looping through them)
        for (int x = 0; x < map.SizeX; x++)
        {
            for (int y = 0; y < map.SizeY; y++)
            {
                Tiles[x, y] = map.Tiles[x, y].Copy();
                BgTiles[x, y] = map.BgTiles[x, y].Copy();

                // Keep track of start tile amount
                if (Tiles[x, y].Type == Tile.START) startCnt++;
            }
        }

        // Copy over BST of buttons
        buttons = map.Buttons.Copy();

        // Copy over list of ziplines
        ziplines = Zipline.CopyList(map.Ziplines);

        // Create/reset camera zoom and position with top left at origin
        Camera = new Camera(Game1._graphics.GraphicsDevice.Viewport);
        Camera.SetPos(new Vector2(Game1.ScreenWidth, Game1.ScreenHeight) / 2);

        // Store camera pos and zoom
        pos = Camera.GetPos();
        zoom = Camera.GetZoom();

        // Set default display options
        ShowFg = true;
        ShowBg = true;
        EditBg = false;
        DisplayGrid = false;

        // Add space to store gridlines
        verLines = new GameLine[map.SizeX + 1];
        horLines = new GameLine[map.SizeY + 1];

        // Create gridlines
        for (int x = 0; x <= map.SizeX; x++) verLines[x] = new GameLine(Game1._graphics.GraphicsDevice, new Vector2(x * Game1.TILE_SIZE * Game1.PIXEL_SCALE, 0), new Vector2(x, map.SizeY) * Game1.TILE_SIZE * Game1.PIXEL_SCALE, GRID_THICKNESS);
        for (int y = 0; y <= map.SizeY; y++) horLines[y] = new GameLine(Game1._graphics.GraphicsDevice, new Vector2(0, y * Game1.TILE_SIZE * Game1.PIXEL_SCALE), new Vector2(map.SizeX, y) * Game1.TILE_SIZE * Game1.PIXEL_SCALE, GRID_THICKNESS);

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

        // Update if edit BG or not, if no layer selected don't let them do anything
        if (!ShowFg && !ShowBg) return;

        // Only check for tile placing if mouse isn't on the bar
        if (IsTileClickable(Game1.GameMousePos(mouse)))
        {
            // Check tile place start
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                // Get tile coordinates of mouse position
                (int tileX, int tileY) = Game1.CalcTile(Camera.ScreenToWorld(Game1.GameMousePos(mouse)));

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
                (int tileX, int tileY) = Game1.CalcTile(Camera.ScreenToWorld(Game1.GameMousePos(mouse)));

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
                            new EditButtonScreen().AddToRoot();

                            // Let the editor know popup is open (so other actions can be blocked)
                            IsEditing = false;
                        }
                    }
                    else
                    {
                        // Store new tile
                        newType = SelectedTile;

                        // If button, calculate new type and only allow placing 1 button at a time
                        if (SelectedTile == Tile.BUTTON)
                        {
                            // Store the rightmost button
                            Button rightmost = buttons.GetRightmost();

                            // Calculate new priority, if first button give it 0
                            newType = Button.PriorityToType(rightmost == null ? 0 : rightmost.Priority + 1);
                            (tileX, tileY) = StartSelected;
                        }

                        // Place selected tile at the tile coordinates (if zipline place start -> end)
                        if (SelectedTile == Tile.ZIPLINE) 
                        {
                            newType = Zipline.IdToType(ziplines.Count, true);
                            PlaceTiles(StartSelected, new Point(tileX, tileY));
                        }
                        else
                        {
                            PlaceTiles(new Point(Math.Min(StartSelected.X, tileX), Math.Min(StartSelected.Y, tileY)), new Point(Math.Max(StartSelected.X, tileX), Math.Max(StartSelected.Y, tileY)));
                        }
                    }
                }
            }

            // If player let go, reset selection (only if editing)
            if (IsEditing && mouse.LeftButton != ButtonState.Pressed)
            {
                StartSelected = NO_TILE_SELECTED;
            }
        }

        // Update list of ziplines
        ReloadZiplines();

        // Check redo input and undo input
        if (kb.IsKeyDown(Keys.LeftControl) && kb.IsKeyDown(Keys.Z) && !prevKb.IsKeyDown(Keys.Z))
        {
            // If they are pressing shift, redo, otherwise undo
            if (kb.IsKeyDown(Keys.LeftShift)) Redo();
            else Undo();
        }

        // Update list of ziplines
        ReloadZiplines();

        // Update undo and redo buttons state
        Game1.editScreen.UndoBtn.IsEnabled = !UndoStack.IsEmpty();
        Game1.editScreen.RedoBtn.IsEnabled = !RedoStack.IsEmpty();

        // Ensure saving is only possible when there is at least 1 start tile (and unsaved)
        Game1.editScreen.SaveBtn.IsEnabled = Game1.editScreen.SaveBtn.IsEnabled = startCnt > 0 && unsaved;
    }

    public void Draw(SpriteBatch spriteBatch, MouseState mouse)
    {
        // Draw each tile, background tile first, 
        for (int x = 0; x < map.SizeX; x++)
        {
            for (int y = 0; y < map.SizeY; y++)
            {
                // Draw background tile first
                if (ShowBg) BgTiles[x, y].Draw(spriteBatch, true);

                // Draw foreground tiles after
                if (ShowFg) Tiles[x, y].Draw(spriteBatch, true);
            }
        }

        // Draw all the ziplines if we are drawing the foreground
        if (ShowFg)
        {
            foreach (Zipline zipline in ziplines)
            {
                zipline.Draw(spriteBatch);
            }
        }

        // Draw tile selection box if hovering over a tile
        if (IsTileClickable(Game1.GameMousePos(mouse)))
        {
            // Store tile coordinates of mouse position
            (int tileX, int tileY) = Game1.CalcTile(Camera.ScreenToWorld(Game1.GameMousePos(mouse)));

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

        // Store scroll change amount
        int scrollDelta = mouse.ScrollWheelValue - prevMouse.ScrollWheelValue;

        // Zoom in and zoom out if scrolling
        if (scrollDelta != 0)
        {
            if (scrollDelta > 0) zoom *= ZOOM_SPEED;
            else zoom /= ZOOM_SPEED;
        }
    }

    private bool IsTileClickable(Vector2 mousePos)
    {
        return IsEditing
            && !Game1.editScreen.BarContainer.IsPointInside(mousePos.X, mousePos.Y)
            && !Game1.editScreen.TopLeftContainer.IsPointInside(mousePos.X, mousePos.Y)
            && !Game1.editScreen.TopRightContainer.IsPointInside(mousePos.X, mousePos.Y);
    }

    private void ExtraRemove(int type, int x, int y, bool canUndo)
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
            
            case Tile.ZIPLINE:
                // Update that ziplines have been edited
                ziplineEdited = true;

                // Store id of this zipline
                int i = Zipline.GetId(type);

                // If index out of bounds, zipline is already removed, do nothing
                if (i >= ziplines.Count) break;

                // Store start and end positions
                Point cur = Zipline.IsStart(type) ? ziplines[i].Start.Pos : ziplines[i].End.Pos;
                Point other = Zipline.IsStart(type) ? ziplines[i].End.Pos : ziplines[i].Start.Pos;
                
                // If the zipline start is already deleted, or end is already deleted, don't need to do anything
                if (x != cur.X || y != cur.Y || i != Zipline.GetId(Tiles[other.X, other.Y].Type)) break;

                // Remove zipline
                ziplines.RemoveAt(i);

                // Remove the other part of the zipline
                if (SelectedTile == Tile.ZIPLINE) ChangeTile(other.X, other.Y, Tile.EMPTY, canUndo: canUndo);
                else ChangeTile(other.X, other.Y, canUndo: canUndo);
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

            case Tile.ZIPLINE:
                // Update that ziplines have been edited
                ziplineEdited = true;
                break;
        }
    }

    private void ChangeTile(int x, int y, int? overwriteType = null, bool? newEditBg = null, bool canUndo = true)
    {
        // Store new type and current tile
        int type = overwriteType ?? newType;
        Tile curTile = (newEditBg ?? EditBg) ? BgTiles[x, y] : Tiles[x, y];
        
        // If tile already changed, don't do anything, if not perform extra remove logic
        if (curTile.Type == type) return;
        ExtraRemove(curTile.Type, x, y, canUndo);

        // Add tile edit to current stack action of changing all selected tiles (also if tile already updated, do nothing)
        if (canUndo) UndoStack.Top().Push(new TileEdit(x, y, newEditBg ?? EditBg, curTile.Type, type));
        else RedoStack.Top().Push(new TileEdit(x, y, newEditBg ?? EditBg, curTile.Type, type));

        // Replace tile and perform any needed updates
        curTile.Type = type;
        ExtraAdd(type, x, y);
    }

    private void PlaceTiles(Point start, Point end)
    {
        // If no selected tile, do nothing
        if (newType == Tile.NULL) return;

        // If the tile is a functional block, then do not place it if trying to be place on bg (unless placing empty tile)
        if (EditBg && EditScreen.Bar == EditScreen.functional && newType != Tile.EMPTY) return;
        
        // Perform zipline place logic
        if (Tile.GetType(newType, true) == Tile.ZIPLINE)
        {
            // If trying to place zipline start and end not at same tile, place the zipline
            if (start != end)
            {
                // Store if start and end are already ziplines or not
                bool startIsZipline = Tile.GetType(Tiles[start.X, start.Y].Type, true) == Tile.ZIPLINE;
                bool endIsZipline = Tile.GetType(Tiles[end.X, end.Y].Type, true) == Tile.ZIPLINE;

                // Return if replacing any ziplines (prevents bugs)
                if (startIsZipline != endIsZipline) return;

                // Add new action
                AddNewAction();

                // // NOTE: Keeping the below for future, when placing ziplines over ziplines is safer / less janky
                // // If we are replacing any ziplines, delete them cleanly (check end again)
                // if (startIsZipline) ChangeTile(start.X, start.Y, Tile.EMPTY);
                // if (endIsZipline) ChangeTile(end.X, end.Y, Tile.EMPTY);

                // Store the end location, place the start
                StartSelected = end;
                ChangeTile(start.X, start.Y, newType);
                ChangeTile(end.X, end.Y, newType + 1);
            }
            return;
        }

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
        UndoStack.Push(new());
        RedoStack.Clear();

        // Update save as not saved
        unsaved = true;
    }

    public void Undo(bool redo = false)
    {
        // Store stack, if nothing to undo, do nothing
        Stack<Stack<TileEdit>> stack = redo ? RedoStack : UndoStack;
        if (stack.IsEmpty()) return;

        // Get the last action and undo it
        Stack<TileEdit> lastAction = stack.Pop();

        // Add new action to the redo stack
        if (!redo) RedoStack.Push(new());
        else UndoStack.Push(new());

        // Undo all tile changes (in reverse to mimic undo one by one)
        while (!lastAction.IsEmpty())
        {
            // Replace tile
            TileEdit edit = lastAction.Pop();
            ChangeTile(edit.X, edit.Y, edit.OldType, edit.Bg, redo);
        }

        // Update save state
        unsaved = true;
    }

    public void Redo()
    {
        // Call same undo logic but on redo stack (undo-ing an undo)
        Undo(true);
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

    private void ReloadZiplines()
    {
        // If ziplines have not been touched, don't do anything, otherwise reset
        if (!ziplineEdited) return;
        ziplineEdited = false;

        // Store all zipline types
        List<(int, int, int)> newZiplines = [];

        // Check all tiles for ziplines
        for (int x = 0; x < map.SizeX; x++)
        {
            for (int y = 0; y < map.SizeY; y++)
            {
                if (Tile.GetType(Tiles[x, y].Type, true) == Tile.ZIPLINE)
                {
                    newZiplines.Add((Tiles[x, y].Type, x, y));
                }
            }
        }

        // Clear the old list of ziplines and sort new one (in descending order by type, i.e. first tuple value)
        ziplines.Clear();
        newZiplines = MergeSort.Sort(newZiplines, (a, b) => b.Item1.CompareTo(a.Item1));

        for (int i = 0; newZiplines.Count != 0; i++)
        {
            // Store and delete start point of this zipline
            (int type, int x, int y) start = newZiplines.Last();
            newZiplines.RemoveAt(newZiplines.Count - 1);

            // Delete start if unpaired or not start type
            if (!Zipline.IsStart(start.type) || newZiplines.Count == 0) 
            {
                Tiles[start.x, start.y].Type = Tile.EMPTY;
                continue;
            }

            // Get end
            (int type, int x, int y) end = newZiplines.Last();

            // If doesn't pair, this start doesn't belong, delete it
            if (start.type + 1 != end.type) 
            {
                Tiles[start.x, start.y].Type = Tile.EMPTY;
                continue;
            }

            // Remove end from list and add new zipline
            newZiplines.RemoveAt(newZiplines.Count - 1);
            ziplines.Add(new Zipline(new Tile(start.x, start.y, Zipline.IdToType(i, true)), new Tile(end.x, end.y, Zipline.IdToType(i, false))));

            // Update zipline tiles' types
            Tiles[start.x, start.y].Type = Zipline.IdToType(i, true);
            Tiles[end.x, end.y].Type = Zipline.IdToType(i, false);
        }
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
            }
        }

        // Save the changed buttons and ziplines back to the map
        map.Buttons = buttons.Copy(); 
        map.Ziplines = Zipline.CopyList(ziplines);

        // Set the map to be saved
        unsaved = false;

        // Update modified date
        map.UpdateModifiedDate();
    }

    private void DisplayDebugInfo(int type)
    {
        // Display debug info depending on type
        switch (type)
        {
            case Tile.BUTTON:
                // Display BSTree of buttons
                Console.WriteLine($"Buttons ({buttons.Count}): {buttons.InOrderTreeDisplay()}");
                break;

            case Tile.ZIPLINE:
                // Display list of ziplines
                Console.Write($"Ziplines ({ziplines.Count}): ");
                foreach (Zipline zipline in ziplines) Console.Write($"({zipline.Start.Type}, {zipline.End.Type}) ");
                Console.WriteLine();
                break;
        }
    }
}