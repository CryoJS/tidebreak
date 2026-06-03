using Gum.Forms.Controls;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class StopEditScreen
    {
        partial void CustomInitialize()
        {
            // Ensure no editing is allowed
            Game1.mapEditor.IsEditing = false;

            // Add logic for button to close popup
            CloseBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // Turn editing back on
                Game1.mapEditor.IsEditing = true;

                this.RemoveFromRoot();
            };

            // Add logic for button to save and leave
            SaveBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // Save
                Game1.mapEditor.Save();

                // Leave
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };

            // Add logic for button to leave without saving
            ExitBtn.Click += (_, _) =>
            {
                // Play click sound and clear screens, open map select screen
                SoundManager.PlayClick();
                GumService.Default.Root.Children.Clear();
                new MapSelectScreen().AddToRoot();
            };
        }
    }
}
