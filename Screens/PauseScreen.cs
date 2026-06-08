// Author:          Jason Sun
// File Name:       PauseScreen.cs
// Project Name:    Tidebreak
// Creation Date:   May 11, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen for the pause menu when playing a map

using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class PauseScreen
    {
        /// <summary>
        /// Sets up screen on creation
        /// </summary>
        partial void CustomInitialize()
        {
            // Clicking continue
            ContinueBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // Close the pause menu
                this.RemoveFromRoot();

                // Unpause
                Game1.paused = false;
            };

            // Click logic for restart button
            RestartBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.PlayMap(Game1.currentMap);
            };

            // Clicking return to map selection
            MapsBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // Change gamestate
                Game1.gameState = Game1.SELECT_MAP;
                
                // Change screen
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };

            // Clicking return to menu
            MenuBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.ReturnToMenu();
            };
        }

        /// <summary>
        /// Checks for user input if the player wants to restart or unpause
        /// </summary>
        /// <param name="kb">Keyboard state this frame</param>
        /// <param name="prevKb">keyboard state one frame ago</param>
        public void Update(KeyboardState kb, KeyboardState prevKb)
        {
            // If the user presses escape to unpause, unpause the game (can't be dead)
            if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape) && !Game1.player.IsDead)
            {
                // Close the pause menu
                this.RemoveFromRoot();

                // Unpause
                Game1.paused = false;
            }

            // If the user presses r, restart the map
            if (kb.IsKeyDown(Keys.R) && !prevKb.IsKeyDown(Keys.R)) Game1.PlayMap(Game1.currentMap);
        }
    }
}
