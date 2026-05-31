using System;
using System.ComponentModel;
using System.Linq;
using Gum.Forms.Controls;
using MonoGameGum;

namespace Tidebreak.Screens
{
    partial class MapSortScreen
    {
        // Store all map sorting options
        private readonly string[] sortingOptions = [
            "Name",
            "Difficulty",
            "Creation Date",
            "Modified Date",
            "Best Time"
        ];

        partial void CustomInitialize()
        {
            // Set item type and add sorting options
            foreach (string option in sortingOptions) 
            {
                ListBoxItem item = new ListBoxItem();
                item.UpdateToObject(option);
                SortCombo.Items.Add(item);
            }

            // Add logic to change map display after selecting an option
            SortCombo.SelectionChanged += (_, args) =>
            {
                // Get selected option, if can't be converted do nothing
                if (args.AddedItems.Count == 0) return;
                string selected = args.AddedItems[0]?.ToString();
                if (selected == null) return;

                // Sort by the selected option and finish
                switch (selected)
                {
                    case "Name":
                        Game1.maps = MergeSort.Sort(Game1.maps, Map.SortByName);
                        break;

                    case "Difficulty":
                        Game1.maps = MergeSort.Sort(Game1.maps, Map.SortByDifficulty);
                        break;

                    case "Creation Date":
                        Game1.maps = MergeSort.Sort(Game1.maps, Map.SortByCreationDate);
                        break;

                    case "Modified Date":
                        Game1.maps = MergeSort.Sort(Game1.maps, Map.SortByModifiedDate);
                        break;

                    case "Best Time":
                        Game1.maps = MergeSort.Sort(Game1.maps, Map.SortByBestTime);
                        break;
                }

                // Find and remove only the MapSelectScreen
                var mapSelect = GumService.Default.Root.Children.First();
                if (mapSelect != null) GumService.Default.Root.Children.Remove(mapSelect);

                // Add a new map select screen (behind this popup)
                GumService.Default.Root.Children.Insert(0, new MapSelectScreen().Visual);
            };

            // Add logic for button to close screen
            CloseBtn.Click += (_, _) =>
            {
                this.RemoveFromRoot();
            };
        }
    }
}
