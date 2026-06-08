// Author:          Jason Sun
// File Name:       TitleScreen.cs
// Project Name:    Tidebreak
// Creation Date:   May 10, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen for the game's title screen

using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class TitleScreen
    {
        /// <summary>
        /// Sets up screen on creation
        /// </summary>
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
