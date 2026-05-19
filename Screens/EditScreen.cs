using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class EditScreen
    {
        partial void CustomInitialize()
        {
            // Save map if there are changes
            SaveBtn.Click += (_, _) =>
            {
                // TODO this and also below logic
            };

            // Stop editing popup when pressing exit
            CloseBtn.Click += (_, _) =>
            {
                StopEditScreen newScreen = new StopEditScreen();
                newScreen.AddToRoot();
            };
        }
    }
}
