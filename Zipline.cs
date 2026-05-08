using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Zipline
{
    // Create zipline constants for appearance
    private const int RADIUS = 100;
    private const int BORDER_WIDTH = 5;
    private const float OPACITY = 0.5f;
    private readonly Color color = Color.Blue;
    private readonly Color borderColor = Color.Black;

    // Create shapes for drawing the zipline
    private GameLine line;
    private GameCircle circle;

    // Store start and end tiles
    public Tile start {set; get;}
    public Tile end {set; get;}

    public Zipline (GraphicsDevice gd)
    {
        line = new GameLine(gd, start.rec.Center.ToVector2(), end.rec.Center.ToVector2(), BORDER_WIDTH);
        circle = new GameCircle(gd, start.rec.Center.ToVector2(), RADIUS, BORDER_WIDTH);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        line.Draw(spriteBatch, color * OPACITY);
        circle.Draw(spriteBatch, color * OPACITY, borderColor);
    }
}