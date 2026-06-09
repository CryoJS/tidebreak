// Author:          Jason Sun
// File Name:       Camera.cs
// Project Name:    Tidebreak
// Creation Date:   May 11, 2026
// Modified Date:   June 8, 2026
// Description:     Handles the camera that follows the player with exponential lerping

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

    /// <summary>
    /// Constructs camera object
    /// </summary>
    /// <param name="viewport">The viewbounds</param>
    public Camera(Viewport viewport)
    {
        camera = new Cam2D(viewport);
        ResetZoom();
    }

    /// <summary>
    /// Gets the center position of the camera
    /// </summary>
    /// <returns>Camera's position</returns>
    public Vector2 GetPos() => camera.GetPosition();

    /// <summary>
    /// Gets the camera's current zoom
    /// </summary>
    /// <returns>Camera's zoom</returns>
    public float GetZoom() => camera.GetZoom();

    /// <summary>
    /// Converts any position on world to equivalent on screen 
    /// </summary>
    /// <param name="pos">Position on world</param>
    /// <returns>Position on screen</returns>
    public Vector2 WorldToScreen(Vector2 pos) => camera.WorldToScreen(pos);

    /// <summary>
    /// Converts any position on screen to equivalent on world
    /// </summary>
    /// <param name="pos">Position on screen</param>
    /// <returns>Position on world</returns>
    public Vector2 ScreenToWorld(Vector2 pos) => camera.ScreenToWorld(pos);

    /// <summary>
    /// Sets the current camera position
    /// </summary>
    /// <param name="newPos">The new position for the camera</param>
    public void SetPos(Vector2 newPos)
    {
        camera.LookAt(newPos);
    }

    /// <summary>
    /// Resets the zoom of the camera to starting value
    /// </summary>
    public void ResetZoom()
    {
        camera.SetZoom(CAMERA_ZOOM);
    }

    /// <summary>
    /// Updates the zoom of the camera
    /// </summary>
    /// <param name="gameTime">Game time for the elapsed seconds in a frame</param>
    /// <param name="zoomGoal">The new zoom the camera should try to achieve</param>
    public void ZoomUpdate(GameTime gameTime, float zoomGoal)
    {
        camera.SetZoom(camera.GetZoom() + (zoomGoal - camera.GetZoom()) * Game1.ExpSmoothing(gameTime, ZOOM_SPEED));
    }

    /// <summary>
    /// Updates the camera's position and zoom
    /// </summary>
    /// <param name="gameTime">Current game time</param>
    /// <param name="goal">The new position the camera should try to achieve</param>
    /// <param name="onZipline">If player is on zipline or not</param>
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

    /// <summary>
    /// Gets the transformation of the camera
    /// </summary>
    /// <returns>Camera's transformation</returns>
    public Matrix GetTransformation()
    {
        return camera.GetTransformation();
    }
}