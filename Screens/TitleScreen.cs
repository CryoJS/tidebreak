using Gum.Forms.Controls;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class TitleScreen
    {
        partial void CustomInitialize()
        {
            // Send player to level selection
            PlayBtn.Click += (_, _) =>
            {
                // Change gamestate
                Game1.gameState = Game1.SELECT_MAP;
                
                // Change screen
                MapSelectScreen newScreen = new MapSelectScreen();
                GumService.Default.Root.Children.Clear();
                newScreen.AddToRoot();
            };

            // TODO settings + credits

            // Exit game
            ExitBtn.Click += (_, _) =>
            {
                Game1.gameState = Game1.EXIT;
            };
        }
    }
}
