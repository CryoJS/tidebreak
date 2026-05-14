using System;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Camera
{
    // Create viewport camera and store settings
    private Cam2D camera;
    private float cameraZoom = 0.5f;
    private float speed = 3f;
    private float deadZone = 100f;

    public Camera(Viewport viewport)
    {
        camera = new Cam2D(viewport);
        camera.SetZoom(cameraZoom);
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
        if (dist.Length() > deadZone)
        {
            // Move camera by a percentage of the distance needed, smoothly
            camera.LookAt(camera.GetPosition() + dist * Game1.ExpSmoothing(gameTime, speed));
        }
    }

    public Matrix GetTransformation()
    {
        return camera.GetTransformation();
    }
}