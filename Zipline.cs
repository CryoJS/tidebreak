// Author:          Jason Sun
// File Name:       Zipline.cs
// Project Name:    Tidebreak
// Creation Date:   April 27, 2026
// Modified Date:   June 8, 2026
// Description:     Handles zipline data and drawing logic

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

    /// <summary>
    /// Constructs a zipline object
    /// </summary>
    public Zipline() {}

    /// <summary>
    /// Constructs a zipline object
    /// </summary>
    /// <param name="start">Zipline start tile</param>
    /// <param name="end">Zipline end tile</param>
    public Zipline(Tile start, Tile end)
    {
        // Store start and end
        Start = start;
        End = end;

        // Load the zipline shapes
        Load();
    }

    /// <summary>
    /// Creates the zipline shapes
    /// </summary>
    public void Load()
    {
        line = new GameLine(Game1._graphics.GraphicsDevice, Start.Rec.Center.ToVector2(), End.Rec.Center.ToVector2(), BORDER_WIDTH);
        startCircle = new GameCircle(Game1._graphics.GraphicsDevice, Start.Rec.Center.ToVector2(), RADIUS, BORDER_WIDTH);
        endCircle = new GameCircle(Game1._graphics.GraphicsDevice, End.Rec.Center.ToVector2(), RADIUS / 2);
    }

    /// <summary>
    /// Copies the current zipline
    /// </summary>
    /// <returns>A deep copy of the zipline</returns>
    public Zipline Copy() => new Zipline(Start.Copy(), End.Copy());

    /// <summary>
    /// Draws the zipline
    /// </summary>
    /// <param name="spriteBatch">Spritebatch to draw to</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        line.Draw(spriteBatch, color);
        startCircle.Draw(spriteBatch, color, borderColor);
        endCircle.Draw(spriteBatch, color);
    }

    /// <summary>
    /// Moves the player
    /// </summary>
    /// <param name="player">The player</param>
    /// <param name="speed">The speed to move the player</param>
    public void MovePlayer(Player player, float speed)
    {
        // Calculate the direction unit vector
        Vector2 dir = End.Rec.Center.ToVector2() - player.rec.Center.ToVector2();
        dir.Normalize();

        // Move player with that speed and direction along the zipline
        player.vel = dir * speed;
    }

    /// <summary>
    /// Copies the list of ziplines
    /// </summary>
    /// <param name="ziplines">The ziplines to copy</param>
    /// <returns>The copied zipline</returns>
    public static List<Zipline> CopyList(List<Zipline> ziplines) => ziplines.Select(zipline => zipline.Copy()).ToList();

    /// <summary>
    /// Find unpaired id of zipline
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>Unpaired id</returns>
    private static int FindUnpairedId(int type)
    {
        return type - Tile.ZIPLINE;
    }

    /// <summary>
    /// Gets the id from type
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>Actual zipline id</returns>
    public static int GetId(int type)
    {
        return FindUnpairedId(type) / 2;
    }

    /// <summary>
    /// If zipline tile is start or not
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>If zipline is start or not</returns>
    public static bool IsStart(int type)
    {
        return (FindUnpairedId(type) & 1) == (Tile.ZIPLINE & 1);
    }

    /// <summary>
    /// Converts zipline id to type
    /// </summary>
    /// <param name="id">Zipline id</param>
    /// <param name="start">If should be zipline start type or end type</param>
    /// <returns>Zipline tile type</returns>
    public static int IdToType(int id, bool start)
    {
        return Tile.ZIPLINE + id * 2 + (start ? 0 : 1);
    }
}