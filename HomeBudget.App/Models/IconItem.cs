using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.Models
{
    public partial class IconItem : ObservableObject
    {
        public string Unicode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [ObservableProperty]
        private bool isSelected;
    }
}
