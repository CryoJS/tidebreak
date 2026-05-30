class TileEdit
{
    // Store action position
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool Bg { get; private set; }

    // Store replaced tile and new tile
    public int OldType { get; private set; }
    public int NewType { get; private set; }

    public TileEdit(int x, int y, bool bg, int oldType, int newType)
    {
        X = x;
        Y = y;
        Bg = bg;
        OldType = oldType;
        NewType = newType;
    }
}