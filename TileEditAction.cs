class TileEdit
{
    // Store action position
    private int x;
    private int y;
    bool editBg;

    // Store replaced tile and new tile
    private int oldType;
    private int newType;

    public TileEdit(int x, int y, bool editBg, int oldType, int newType)
    {
        this.x = x;
        this.y = y;
        this.editBg = editBg;
        this.oldType = oldType;
        this.newType = newType;
    }
}