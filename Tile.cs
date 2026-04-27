class Tile
{
    // Create constants for tile types
    public const int EMPTY = 0;
    public const int WATER = 1;

    private int type;
    int pos = 0; // FIXME how to store w/ camera

    public Tile(int type = EMPTY)
    {
        this.type = type;
    }
}