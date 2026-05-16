using System;
using Microsoft.Xna.Framework;

class Button : IComparable<Button>
{
    // Store button location and priority
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Priority { get; set; }

    // Store center of button
    public Vector2 Center { get; private set; }

    public Button(int x, int y, int priority, Vector2 center)
    {
        X = x;
        Y = y;
        Priority = priority;
        Center = center;
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
}