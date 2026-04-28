using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Tile
{
    // Create constants for tile types
    public const int TYPE_AMOUNT = 24;

    public const int EMPTY = -1;
    public const int WATER = 0;
    public const int GRASS = 1;
    public const int SNOW = 2;
    public const int SAND = 3;
    public const int DIRT = 4;
    public const int STONE = 5;
    public const int METAL = 6;
    public const int GOLD = 7;
    public const int PLANK = 8;
    public const int PIKE = 9;
    public const int CRATE = 10;
    public const int LEAF = 11;

    public const int SIGN = 12;
    public const int LEFT_SIGN = 13;
    public const int RIGHT_SIGN = 14;
    public const int FERN = 15;
    public const int CACTUS = 16;
    public const int TREE = 17;
    public const int TRUNK = 18;

    // Create constants for special tile types
    public const int START = 19;
    public const int END = 20;
    public const int BUTTON = 21;
    public const int PRESSED_BUTTON = 22;
    public const int LADDER = 23;

    // Store graphic information
    public static Texture2D[] tileTextures = new Texture2D[TYPE_AMOUNT];

    private int type;
    private int posX;
    private int posY;

    public Tile(int posX, int posY, int type = EMPTY)
    {
        this.type = type;
        this.posX = posX;
        this.posY = posY;
    }

    public void Draw(SpriteBatch _spriteBatch, bool editing = false)
    {
        // Draw the tile if it can be drawn and should be visible
        if (type != EMPTY && (editing || (type != START && type != END)))
        {
            _spriteBatch.Draw(tileTextures[type], new Vector2(posX, posY), Color.White);
        }
    }
}