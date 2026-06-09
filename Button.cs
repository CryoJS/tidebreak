// Author:          Jason Sun
// File Name:       Button.cs
// Project Name:    Tidebreak
// Creation Date:   May 13, 2026
// Modified Date:   June 8, 2026
// Description:     Stores values for the button in a map

using System;
using Microsoft.Xna.Framework;
using Tidebreak;

class Button : IComparable<Button>
{
    // Store button location and priority
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Priority { get; set; }

    // Store center of button
    public Vector2 Center { get; private set; }

    /// <summary>
    /// Constructs a button
    /// </summary>
    /// <param name="x">The x tile position</param>
    /// <param name="y">The y tile position</param>
    /// <param name="priority">The priority of the button, lower means earlier</param>
    public Button(int x, int y, int priority)
    {
        // Store given parameters
        X = x;
        Y = y;
        Priority = priority;

        // Calculate scale amount and calculate center
        float scale = Game1.TILE_SIZE * Game1.PIXEL_SCALE;
        Center = new Vector2(x * scale + scale / 2, y * scale + scale / 2);
    }

    /// <summary>
    /// Compares this button with another
    /// </summary>
    /// <param name="other">The other button to compare with</param>
    /// <returns>Comparing this w/ another in that order: -1 if less than, 0 if equal, 1 if greater than</returns>
    public int CompareTo(Button other)
    {
        return Priority.CompareTo(other.Priority);
    }
    
    /// <summary>
    /// Check if same type, and if same type check if equal priority
    /// </summary>
    /// <param name="other">The other object to compare with</param>
    /// <returns>If equal or not</returns>
    public override bool Equals(object other)
    {
        return other is Button && Priority == ((Button)other).Priority;
    }

    /// <summary>
    /// Gets the hash code of the priority
    /// </summary>
    /// <returns>Hash code of priority value</returns>
    public override int GetHashCode()
    {
        return Priority.GetHashCode();
    }

    /// <summary>
    /// Converts tile type value to priority value
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>Priority value</returns>
    public static int TypeToPriority(int type)
    {
        return -(type - (int)Tile.Func.ButtonStart);
    }

    /// <summary>
    /// Converts priority value to tile type value
    /// </summary>
    /// <param name="priority">Priority value</param>
    /// <returns>Tile type</returns>
    public static int PriorityToType(int priority)
    {
        return (int)Tile.Func.ButtonStart - priority;
    }
}