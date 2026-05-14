using System;
using Microsoft.Xna.Framework;

class Button : IComparable<Button>
{
    // Store button location and priority
    public int x { get; private set; }
    public int y { get; private set; }
    public int priority { get; set; }

    // Store center of button
    public Vector2 center { get; private set; }

    public Button(int x, int y, int priority, Vector2 center)
    {
        this.x = x;
        this.y = y;
        this.priority = priority;
        this.center = center;
    }

    public int CompareTo(Button other)
    {
        return priority.CompareTo(other.priority);
    }
    
    // Check if same type, and if same type check if equal priority
    public override bool Equals(object other)
    {
        return other is Button && priority == ((Button)other).priority;
    }

    public override int GetHashCode()
    {
        return priority.GetHashCode();
    }
}