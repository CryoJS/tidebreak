// Author:          Jason Sun
// File Name:       PlayScreen.cs
// Project Name:    Tidebreak
// Creation Date:   May 10, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen (HUD) when playing a map

using System;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class PlayScreen
    {
        /// <summary>
        /// Sets up screen on creation
        /// </summary>
        partial void CustomInitialize()
        {
            // Play map load effect
            SoundManager.PlayLoad();

            // Store best time from current map
            float bestTime = Game1.currentMap.BestTime;

            // Store current screen in Game1.cs
            Game1.playScreen = this;

            // Update map name text
            NameText.Text = Game1.currentMap.Name;

            // Update current map time
            TimeText.SetBinding(nameof(TimeText.Text), nameof(Game1.currentMap.Time));

            // Update map best time text
            BestTimeText.Text = Math.Round(bestTime) == Map.EMPTY ? Map.MISSING_BEST_TIME : Game1.FormatTime(bestTime);

            // Update button indicator visibility
            BtnIndicator.Visible = Game1.currentMap.Buttons.Count > 0;

            // Clicking pause button
            PauseBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                TriggerPause();
            };
        }

        /// <summary>
        /// Updates HUD display with latest player and map data
        /// </summary>
        /// <param name="kb">Keyboard state this frame</param>
        /// <param name="prevKb">keyboard state one frame ago</param>
        public void Update(KeyboardState kb, KeyboardState prevKb)
        {
            // Update current map time
            TimeText.Text = Game1.FormatTime(Game1.currentMap.Time, false);

            // Update oxygen bar
            OxygenBar.BarPercent = 100 * Game1.player.Oxygen / Player.MAX_OXYGEN;

            // Update vignette intensity by how much oxygen is left
            Vignette.Alpha2 = (int)(255 * (1 - Math.Max(0, Game1.player.Oxygen) / Player.MAX_OXYGEN));

            // If the user presses escape to pause, pause the game (if not dead)
            if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape) && !Game1.player.IsDead) TriggerPause();
        }

        /// <summary>
        /// Opens the pause screen and pauses the game
        /// </summary>
        private void TriggerPause()
        {
            // Add pause menu options screen popup
            Game1.pauseScreen = new PauseScreen();
            Game1.pauseScreen.AddToRoot();

            // Change pause state
            Game1.paused = true;
        }
    }
}
