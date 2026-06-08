// Author:          Jason Sun
// File Name:       AboutScreen.cs
// Project Name:    Tidebreak
// Creation Date:   June 2, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen for the game's about page

using Gum.Forms.Controls;

namespace Tidebreak.Screens
{
    partial class AboutScreen
    {
        // Store default scroll speed
        private const int DEFAULT_SCROLL_SPEED = 50;

        /// <summary>
        /// Sets up screen on creation
        /// </summary>
        partial void CustomInitialize()
        {
            // Increase scroll speed
            SectionList.VerticalScrollBarInstance.SmallChange = DEFAULT_SCROLL_SPEED;

            // Add logic for return
            ReturnBtn.Click += (_, _) =>
            {
                SoundManager.PlayClick();
                this.RemoveFromRoot();
            };
        }
    }
}
