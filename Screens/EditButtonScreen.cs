using System;
using Gum.Forms.Controls;

namespace Tidebreak.Screens
{
    partial class EditButtonScreen
    {
        partial void CustomInitialize()
        {
            // Store priority input
            int priority;

            // Ensure priority input only recieves digits (to be an integer)
            InputPriority.PreviewTextInput += Game1.IntegerOnlyHandler;

            // Display current button priority
            Priority.Text = Button.TypeToPriority(Game1.mapEditor.Tiles[MapEditor.StartSelected.X, MapEditor.StartSelected.Y].Type).ToString();

            // Add logic for closing popup
            CloseBtn.Click += (_, _) =>
            {
                Close();
            };

            // Add logic for saving button setting
            SaveBtn.Click += (_, _) =>
            {
                // Trim input and store
                InputPriority.Text = InputPriority.Text.Trim();
                priority = Convert.ToInt32(InputPriority.Text);

                // If valid integer, save it and close popup
                if (priority >= 0)
                {
                    Game1.mapEditor.ChangeButtonSettings(priority);
                    Close();
                }
            };
        }

        // NOTE: Remove popup and let editor know can resume editing
        private void Close()
        {
            this.RemoveFromRoot();
            Game1.mapEditor.IsEditing = true;
        }
    }
}
