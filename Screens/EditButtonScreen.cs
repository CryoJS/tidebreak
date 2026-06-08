// Author:          Jason Sun
// File Name:       EditButtonScreen.cs
// Project Name:    Tidebreak
// Creation Date:   May 27, 2026
// Modified Date:   June 7, 2026
// Description:     GUI screen for the popup when user edits a button's priority in the map editor

using System;
using Gum.Forms.Controls;

namespace Tidebreak.Screens
{
    partial class EditButtonScreen
    {
        /// <summary>
        /// Sets up screen on creation
        /// </summary>
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
                SoundManager.PlayClick();
                Close();
            };

            // Add logic for saving button setting
            SaveBtn.Click += (_, _) =>
            {
                // Play click sound
                SoundManager.PlayClick();

                // Trim input and store
                InputPriority.Text = InputPriority.Text.Trim();
                
                // Try to parse
                try
                {
                    // Convert
                    priority = Convert.ToInt32(InputPriority.Text);

                    // If valid integer, save it and close popup
                    if (priority >= 0)
                    {
                        Game1.mapEditor.ChangeButtonSettings(priority);
                        Close();
                    }
                }
                catch
                {
                    Console.WriteLine("ERROR - Invalid button priority");
                }
            };
        }

        /// <summary>
        /// Closes popup: Remove the UI and lets the editor know that the user can resume editing
        /// </summary>
        private void Close()
        {
            this.RemoveFromRoot();
            Game1.mapEditor.IsEditing = true;
        }
    }
}
