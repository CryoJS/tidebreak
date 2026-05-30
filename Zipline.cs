using System;
using System.Collections.Generic;
using System.Linq;
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
    public Tile Start { set; get; }
    public Tile End { set; get; }

    public Zipline() {}

    public Zipline(Tile start, Tile end)
    {
        Start = start;
        End = end;

        // Load the zipline shapes
        Load();
    }

    public void Load()
    {
        line = new GameLine(Game1._graphics.GraphicsDevice, Start.Rec.Center.ToVector2(), End.Rec.Center.ToVector2(), BORDER_WIDTH);
        startCircle = new GameCircle(Game1._graphics.GraphicsDevice, Start.Rec.Center.ToVector2(), RADIUS, BORDER_WIDTH);
        endCircle = new GameCircle(Game1._graphics.GraphicsDevice, End.Rec.Center.ToVector2(), RADIUS / 2);
    }

    public Zipline Copy() => new Zipline(Start.Copy(), End.Copy());

    public void Draw(SpriteBatch spriteBatch)
    {
        line.Draw(spriteBatch, color);
        startCircle.Draw(spriteBatch, color, borderColor);
        endCircle.Draw(spriteBatch, color);
    }

    public void MovePlayer(Player player, float speed)
    {
        // Calculate the direction unit vector
        Vector2 dir = End.Rec.Center.ToVector2() - player.rec.Center.ToVector2();
        dir.Normalize();

        // Move player with that speed and direction along the zipline
        player.vel = dir * speed;
    }

    public static List<Zipline> CopyList(List<Zipline> ziplines) => ziplines.Select(zipline => zipline.Copy()).ToList(); // REVIEW long arrow func

    private static int FindUnpairedId(int type)
    {
        return type - Tile.ZIPLINE;
    }

    public static int GetId(int type)
    {
        return FindUnpairedId(type) / 2;
    }

    public static bool IsStart(int type)
    {
        return (FindUnpairedId(type) & 1) == (Tile.ZIPLINE & 1);
    }

    public static int IdToType(int id, bool start)
    {
        return Tile.ZIPLINE + id * 2 + (start ? 0 : 1);
    }
}