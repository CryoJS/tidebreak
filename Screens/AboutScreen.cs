using Gum.Forms.Controls;

namespace Tidebreak.Screens
{
    partial class AboutScreen
    {
        // Store default scroll speed
        private const int DEFAULT_SCROLL_SPEED = 50;

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
