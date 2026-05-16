using System;
using Gum.Forms.DefaultVisuals;
using Gum.Mvvm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class PlayScreen
    {
        partial void CustomInitialize()
        {
            // Store best time from current map
            float bestTime = Game1.currentMap.bestTime;

            // Store current screen in Game1.cs
            Game1.playScreen = this;

            // Update map name text
            NameText.Text = Game1.currentMap.name;

            // Update current map time
            TimeText.SetBinding(nameof(TimeText.Text), nameof(Game1.currentMap.time));

            // Update map best time text
            if (Math.Round(bestTime) == Map.EMPTY)
            {
                BestTimeText.Text = Map.MISSING_BEST_TIME;
            }
            else
            {
                BestTimeText.Text = Game1.FormatTime(bestTime);
            }

            // Clicking pause button
            PauseBtn.Click += (_, _) => TriggerPause();
        }

        public void Update(KeyboardState kb, KeyboardState prevKb)
        {
            // Update current map time
            TimeText.Text = Game1.FormatTime(Game1.currentMap.time, false);

            // Update oxygen bar
            OxygenBar.BarPercent = 100 * Game1.player.Oxygen / Player.MAX_OXYGEN;

            // Update vignette intensity by how much oxygen is left
            Vignette.Alpha2 = (int)(255 * (1 - Math.Max(0, Game1.player.Oxygen) / Player.MAX_OXYGEN));

            // If the user presses escape to pause, pause the game
            if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape)) TriggerPause();
        }

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
