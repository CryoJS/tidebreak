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

    public Button(int x, int y, int priority)
    {
        X = x;
        Y = y;
        Priority = priority;

        // Calculate scale amount and calculate center
        float scale = Game1.TILE_SIZE * Game1.PIXEL_SCALE;
        Center = new Vector2(x * scale + scale / 2, y * scale + scale / 2);
    }

    public int CompareTo(Button other)
    {
        return Priority.CompareTo(other.Priority);
    }
    
    // Check if same type, and if same type check if equal priority
    public override bool Equals(object other)
    {
        return other is Button && Priority == ((Button)other).Priority;
    }

    public override int GetHashCode()
    {
        return Priority.GetHashCode();
    }

    public static int TypeToPriority(int type)
    {
        return -(type - Tile.BUTTON_START);
    }

    public static int PriorityToType(int priority)
    {
        return Tile.BUTTON_START - priority;
    }
}