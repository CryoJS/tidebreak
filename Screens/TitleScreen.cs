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
                SoundManager.PlayClick();

                // Change gamestate
                Game1.gameState = Game1.SELECT_MAP;
                
                // Change screen
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };

            // Add the about screen when pressing btn
            AboutBtn.Click += (_, _) =>
            {
                new AboutScreen().AddToRoot();
            };

            // Add the settings screen when pressing btn
            SettingsBtn.Click += (_, _) =>
            {
                new SettingsScreen().AddToRoot();
            };

            // Exit game
            ExitBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                Game1.gameState = Game1.EXIT;
            };
        }
    }
}
