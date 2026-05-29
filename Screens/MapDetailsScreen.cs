using Gum.Converters;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using RenderingLibrary.Graphics;

using System.Linq;

namespace Tidebreak.Screens
{
    partial class MapDetailsScreen
    {
        private const string NO_BEST_TIME = "None";

        partial void CustomInitialize()
        {
            // Only update all text if map selected is not null
            if (Game1.currentMap != null)
            {
                // Update details
                MapName.Text = Game1.currentMap.Name;
                Author.Text = Game1.currentMap.Author;
                Size.Text = $"{Game1.currentMap.SizeX} x {Game1.currentMap.SizeY}";
                Locked.Text = Game1.currentMap.Locked ? "Yes" : "No";

                // Change text and color of difficulty text
                Difficulty.Text = Game1.currentMap.Difficulty.ToString("F1");
                Difficulty.Color = Map.diffColors[(int)Game1.currentMap.Difficulty];

                // Update dates
                CreationDate.Text = Game1.currentMap.CreationDate.ToLongDateString();
                ModifiedDate.Text = Game1.currentMap.ModifiedDate.ToLongDateString();

                // Update best time and description
                if (Game1.currentMap.BestTime == Map.EMPTY) BestTime.Text = NO_BEST_TIME;
                else BestTime.Text = Game1.FormatTime(Game1.currentMap.BestTime);
                
                // Update description
                Description.Text = Game1.currentMap.Description;
            }

            // Add close popup button option
            CloseBtn.Click += (_, _) =>
            {
                this.RemoveFromRoot();
            };
        }
    }
}
