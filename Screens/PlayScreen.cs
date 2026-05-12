using System;
using Gum.Forms.DefaultVisuals;
using Gum.Mvvm;
using Microsoft.Xna.Framework;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class PlayScreen
    {
        partial void CustomInitialize()
        {
            // Store current map and play map data
            Map map = Game1.maps[Game1.currentMap];
            float bestTime = map.bestTime;

            // Store current screen in Game1.cs
            Game1.playScreen = this;

            // Update map name text
            NameText.Text = map.name;

            // Update current map time
            TimeText.SetBinding(nameof(TimeText.Text), nameof(map.time));

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
            PauseBtn.Click += (_, _) =>
            {
                // Add pause menu options screen popup
                PauseScreen newScreen = new PauseScreen();
                newScreen.AddToRoot();

                // Change pause state
                Game1.paused = true;
            };
        }

        public void Update()
        {
            // Update current map time
            TimeText.Text = Game1.FormatTime(Game1.maps[Game1.currentMap].time, false);

            // Update oxygen bar
            OxygenBar.BarPercent = 100 * Game1.player.oxygen / Player.MAX_OXYGEN;
        }
    }
}
