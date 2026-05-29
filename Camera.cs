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

    public Vector2 GetPos()
    {
        return camera.GetPosition();
    }

    public float GetZoom()
    {
        return camera.GetZoom();
    }

    // REVIEW can i use arrow func, also do i need to document stuff like this?
    public Vector2 WorldToScreen(Vector2 pos) => camera.WorldToScreen(pos);
    public Vector2 ScreenToWorld(Vector2 pos) => camera.ScreenToWorld(pos);

    public void SetPos(Vector2 newPos)
    {
        camera.LookAt(newPos);
    }

    public void ResetZoom()
    {
        camera.SetZoom(CAMERA_ZOOM);
    }

    public void ZoomUpdate(GameTime gameTime, float zoomGoal)
    {
        camera.SetZoom(camera.GetZoom() + (zoomGoal - camera.GetZoom()) * Game1.ExpSmoothing(gameTime, ZOOM_SPEED));
    }

    public void Update(GameTime gameTime, Vector2 goal, bool onZipline = false)
    {
        // Store needed distance for camera to travel, and speed
        Vector2 dist = goal - camera.GetPosition();

        // Only update camera if outside of dead zone
        if (dist.Length() > DEAD_ZONE)
        {
            // Move camera by a percentage of the distance needed, smoothly
            camera.LookAt(camera.GetPosition() + dist * Game1.ExpSmoothing(gameTime, FOLLOW_SPEED));
        }

        // Smooth zoom to desired zoom depending if on zipline or not (zoom out on ziplines)
        ZoomUpdate(gameTime, onZipline ? ZIPLINE_CAMERA_ZOOM : CAMERA_ZOOM);
    }

    public Matrix GetTransformation()
    {
        return camera.GetTransformation();
    }
}