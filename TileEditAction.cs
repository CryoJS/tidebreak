// Author:          Jason Sun
// File Name:       TileEditAction.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     Stores the changes done to a map, from old tile to new tile

class TileEdit
{
    // Store action position
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool Bg { get; private set; }

    // Store replaced tile and new tile
    public int OldType { get; private set; }
    public int NewType { get; private set; }

    /// <summary>
    /// Creates a tile edit object
    /// </summary>
    /// <param name="x">Tile x position</param>
    /// <param name="y">Tile y position</param>
    /// <param name="bg">If editing background</param>
    /// <param name="oldType">Old/previous type</param>
    /// <param name="newType">New type to change to</param>
    public TileEdit(int x, int y, bool bg, int oldType, int newType)
    {
        X = x;
        Y = y;
        Bg = bg;
        OldType = oldType;
        NewType = newType;
    }
}