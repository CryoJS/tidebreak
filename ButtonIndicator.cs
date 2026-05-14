using System;
using Microsoft.Xna.Framework;
using Tidebreak;

class ButtonIndicator
{
    private const float RADIUS = 350;
    private const float INNER_DIST = RADIUS + 300;
    private const float OUTER_DIST = RADIUS + 800;

    public ButtonIndicator() {}

    public void Update(Player player)
    {
        // Only show indicator if there is a next button
        if (player.nextButton != null)
        {
            // Find the direction and distance to the next button
            Vector2 direction = player.nextButton.center - player.rec.Center.ToVector2();
            float dist = direction.Length();

            // If the distance is far enough, draw indicator
            if (dist > INNER_DIST)
            {
                // Make direction the correct distance
                direction.Normalize();
                direction *= RADIUS;

                // Update indicator position
                Game1.playScreen.BtnIndicator.X = Game1.screenWidth / 2 + direction.X;
                Game1.playScreen.BtnIndicator.Y = Game1.screenHeight / 2 + direction.Y;

                // Update indicator rotation
                float angle = MathHelper.ToDegrees((float)Math.Atan2(-direction.Y, direction.X));
                Game1.playScreen.BtnIndicator.Rotation = angle;

                // Make indicator transparent depending on distance
                Game1.playScreen.BtnIndicator.Alpha = (int)(255 * Math.Min(1, (dist - INNER_DIST) / (OUTER_DIST - INNER_DIST)));

                // Make the indicator visible
                Game1.playScreen.BtnIndicator.Visible = true;
            }
            else
            {
                // Remove the indicator
                Game1.playScreen.BtnIndicator.Visible = false;
            }
        }
    }
}