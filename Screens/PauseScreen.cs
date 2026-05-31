using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class PauseScreen
    {
        partial void CustomInitialize()
        {
            // Clicking continue
            ContinueBtn.Click += (_, _) =>
            {
                // Close the pause menu
                this.RemoveFromRoot();

                // Unpause
                Game1.paused = false;
            };

            // Click logic for restart button
            RestartBtn.Click += (_, _) => Game1.PlayMap(Game1.currentMap);

            // Clicking return to map selection
            MapsBtn.Click += (_, _) =>
            {
                // Change gamestate
                Game1.gameState = Game1.SELECT_MAP;
                
                // Change screen
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };

            // Clicking return to menu
            MenuBtn.Click += (_, _) =>
            {
                Game1.ReturnToMenu();
            };
        }

        public void Update(KeyboardState kb, KeyboardState prevKb)
        {
            // If the user presses escape to unpause, unpause the game
            if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape))
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
