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
            if (Game1.selectedMap != null)
            {
                // Update details
                MapName.Text = Game1.selectedMap.name;
                Author.Text = Game1.selectedMap.author;
                Locked.Text = Game1.selectedMap.locked ? "Yes" : "No";

                // Change text and color of difficulty text
                Difficulty.Text = Game1.selectedMap.difficulty.ToString("F1");
                Difficulty.Color = Map.diffColors[(int)Game1.selectedMap.difficulty];

                // Update dates
                CreationDate.Text = Game1.selectedMap.creationDate.ToLongDateString();
                ModifiedDate.Text = Game1.selectedMap.modifiedDate.ToLongDateString();

                // Update best time and description
                if (Game1.selectedMap.bestTime == Map.EMPTY) BestTime.Text = NO_BEST_TIME;
                else BestTime.Text = Game1.FormatTime(Game1.selectedMap.bestTime);
                
                // Update description
                Description.Text = Game1.selectedMap.description;
            }

            // Add close popup button option
            CloseBtn.Click += (_, _) =>
            {
                this.RemoveFromRoot();
            };
        }
    }
}
