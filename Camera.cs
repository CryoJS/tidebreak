using System;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Camera
{
    // Store camera settings
    private const float CAMERA_ZOOM = 0.5f;
    private const float SPEED = 3f;
    private const float DEAD_ZONE = 100f;

    // Create viewport camera
    private Cam2D camera; 

    public Camera(Viewport viewport)
    {
        camera = new Cam2D(viewport);
        camera.SetZoom(CAMERA_ZOOM);
    }

    public Vector2 WorldToScreen(Vector2 pos)
    {
        return camera.WorldToScreen(pos);
    }

    public void SetPos(Vector2 newPos)
    {
        camera.LookAt(newPos);
    }

    public void Update(GameTime gameTime, Rectangle rec)
    {
        // Store needed distance for camera to travel, and speed
        Vector2 dist = rec.Center.ToVector2() - camera.GetPosition();

        // Only update camera if outside of dead zone
        if (dist.Length() > DEAD_ZONE)
        {
            // Move camera by a percentage of the distance needed, smoothly
            camera.LookAt(camera.GetPosition() + dist * Game1.ExpSmoothing(gameTime, SPEED));
        }
    }

    public Matrix GetTransformation()
    {
        return camera.GetTransformation();
    }
}