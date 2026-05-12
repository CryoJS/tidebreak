using System;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            // Move camera by a percentage of the distance needed, smoothly (source: https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/)
            camera.LookAt(camera.GetPosition() + dist * (1 - MathF.Exp(-speed * (float)gameTime.ElapsedGameTime.TotalSeconds)));
        }
    }

    public Matrix GetTransformation()
    {
        return camera.GetTransformation();
    }
}