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
                // Turn editing back on
                Game1.mapEditor.IsEditing = true;

                this.RemoveFromRoot();
            };

            // Add logic for button to save and leave
            SaveBtn.Click += (_, _) =>
            {
                // Save
                Game1.mapEditor.Save();

                // Leave
                MapSelectScreen newScreen = new MapSelectScreen();
                GumService.Default.Root.Children.Clear();
                newScreen.AddToRoot();
            };

            // Add logic for button to leave without saving
            ExitBtn.Click += (_, _) =>
            {
                MapSelectScreen newScreen = new MapSelectScreen();
                GumService.Default.Root.Children.Clear();
                newScreen.AddToRoot();
            };
        }
    }
}
