using System;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class WinScreen
    {
        private const string TIME_MESSAGE = "Time taken: ";
        
        partial void CustomInitialize()
        {
            // Store current map
            Map map = Game1.currentMap;

            // Update time taken text
            H2.Text = TIME_MESSAGE + Game1.FormatTime(map.Time);

            // If new best time, show new best time UI and update best time, otherwise show normal win UI
            if (map.BestTime == Map.EMPTY || map.Time < map.BestTime)
            {
                NewBestEffect.Visible = true;
                WinVignette.Visible = false;
                map.BestTime = map.Time;
            }
            else
            {
                NewBestEffect.Visible = false;
                WinVignette.Visible = true;
            }

            // Click logic for restart button
            RestartBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                // Play the current map again
                Game1.PlayMap(Game1.currentMap);
            };

            // Click logic for return to map selection menu button
            MapsBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                // Change gamestate
                Game1.gameState = Game1.SELECT_MAP;
                
                // Change screen
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };

            // Click logic for return to menu button
            MenuBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();
                Game1.ReturnToMenu();
            };
        }
    }
}
