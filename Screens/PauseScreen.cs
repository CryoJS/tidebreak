using Gum.Forms.Controls;
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

            // Clicking return to map selection
            MapsBtn.Click += (_, _) =>
            {
                // Change screen
                MapSelectScreen newScreen = new MapSelectScreen();
                GumService.Default.Root.Children.Clear();
                newScreen.AddToRoot();

                // Change gamestate
                Game1.gameState = Game1.SELECT_MAP;
            };

            // Clicking return to menu
            MenuBtn.Click += (_, _) =>
            {
                Game1.ReturnToMenu();
            };
        }
    }
}
