// Author:          Jason Sun
// File Name:       DeathScreen.cs
// Project Name:    Tidebreak
// Creation Date:   May 12, 2026
// Modified Date:   June 7, 2026
// Description:     The GUI screen popup when the player dies

using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class DeathScreen
    {
        // Store random death messages
        private static readonly string[] deathMessages =
        {
            "You went into the light.",
            "You got trolled...",
            "You just should've not died lol.",
            "Skill issue.",
            "Player.Health = 0",
            "Did you slip on a banana peel?",
            "Oof, you got the bad ending!",
            "You're supposed to escape the flood, not chase it.",
            "You'll be fine, just walk it off.",
            "Try again! You might do better... or worse...",
            "Got turned into dust huh.",
            "\"Failure: A temporary state where\nthe most valuable lessons are learned.\"",
            "You were doomed to fail...",
            "RIP, you will be missed.",
            "Cooked a little too hard.",
            "You might need a little bit more practice.",
            "Did you just pull a Ludwig? Such a LOLCOW.",
            "Unlucky...",
            "You inhaled incorrectly...",
            "You became one with the waters...",
            "Oh no, try harder next time.",
            "Maybe focus better next time! :skull:",
            "Not even Mr. Lane can save you...",
            "Dreams... shattered.",
            "You drowned...",
            "All that progress... down the drain...",
            "Did you forget your life jacket?",
            "You had a bit too big of a sip of water.",
            "You must've tried to inhale water. It's not good for u BTW."
        };

        /// <summary>
        /// Sets up screen on creation
        /// </summary>
        partial void CustomInitialize()
        {
            // Set a random death message
            H2.Text = deathMessages[Game1.rng.Next(0, deathMessages.Length)];

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
