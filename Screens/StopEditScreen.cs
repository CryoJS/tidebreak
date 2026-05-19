using Gum.Forms.Controls;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class StopEditScreen
    {
        partial void CustomInitialize()
        {
            // Add logic for button to close popup
            CloseBtn.Click += (_, _) =>
            {
                this.RemoveFromRoot();
            };

            // Add logic for button to leave with saving
            SaveBtn.Click += (_, _) =>
            {
                // TODO
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
