using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Zipline
{
    // Create zipline constants for appearance
    private const int RADIUS = 50;
    private const int BORDER_WIDTH = 10;
    private readonly Color color = Color.Black;
    private readonly Color borderColor = Color.Green;

    // Create shapes for drawing the zipline
    private GameLine line;
    private GameCircle startCircle;
    private GameCircle endCircle;

    // Store start and end tiles
    public Tile start { set; get; }
    public Tile end { set; get; }

    public Zipline() { }

    public void Load(GraphicsDevice gd)
    {
        line = new GameLine(gd, start.rec.Center.ToVector2(), end.rec.Center.ToVector2(), BORDER_WIDTH);
        startCircle = new GameCircle(gd, start.rec.Center.ToVector2(), RADIUS, BORDER_WIDTH);
        endCircle = new GameCircle(gd, end.rec.Center.ToVector2(), RADIUS / 2);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        line.Draw(spriteBatch, color);
        startCircle.Draw(spriteBatch, color, borderColor);
        endCircle.Draw(spriteBatch, color);
    }

    public void MovePlayer(Player player, float speed)
    {
        // Calculate the direction unit vector
        Vector2 dir = end.rec.Center.ToVector2() - player.rec.Center.ToVector2();
        dir.Normalize();

        // Move player with that speed and direction along the zipline
        player.vel = dir * speed;
    }

    private static int FindUnpairedId(int type)
    {
        return type - Tile.ZIPLINE;
    }

    public static int FindId(int type)
    {
        return FindUnpairedId(type) / 2;
    }

    public static bool IsStart(int type)
    {
        return (FindUnpairedId(type) & 1) == (Tile.ZIPLINE & 1);
    }
}