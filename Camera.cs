using System;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tidebreak;

class Camera
{
    // Store camera settings
    private const float FOLLOW_SPEED = 3f;
    private const float DEAD_ZONE = 100f;

    private const float ZOOM_SPEED = 1.5f;
    private const float CAMERA_ZOOM = 0.5f;
    private const float ZIPLINE_CAMERA_ZOOM = 0.4f;

    // Create viewport camera and store current zoom
    private Cam2D camera; 

    public Camera(Viewport viewport)
    {
        camera = new Cam2D(viewport);
        ResetZoom();
    }

    public Vector2 WorldToScreen(Vector2 pos)
    {
        return camera.WorldToScreen(pos);
    }

    public void SetPos(Vector2 newPos)
    {
        camera.LookAt(newPos);
    }

    public void ResetZoom()
    {
        camera.SetZoom(CAMERA_ZOOM);
    }

    public void Update(GameTime gameTime, Rectangle rec, bool onZipline)
    {
        // Store needed distance for camera to travel, and speed
        Vector2 dist = rec.Center.ToVector2() - camera.GetPosition();

        // Only update camera if outside of dead zone
        if (dist.Length() > DEAD_ZONE)
        {
            // Move camera by a percentage of the distance needed, smoothly
            camera.LookAt(camera.GetPosition() + dist * Game1.ExpSmoothing(gameTime, FOLLOW_SPEED));
        }

        // Smooth zoom to desired zoom depending if on zipline or not (zoom out on ziplines)
        camera.SetZoom(camera.GetZoom() + ((onZipline ? ZIPLINE_CAMERA_ZOOM : CAMERA_ZOOM) - camera.GetZoom()) * Game1.ExpSmoothing(gameTime, ZOOM_SPEED));
    }

    public Matrix GetTransformation()
    {
        return camera.GetTransformation();
    }
}