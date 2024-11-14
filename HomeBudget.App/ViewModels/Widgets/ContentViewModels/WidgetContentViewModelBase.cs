using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.ViewModels.Interfaces;

namespace HomeBudget.App.ViewModels.Widgets
{
    public abstract partial class WidgetContentViewModelBase : ObservableObject, IConfigurable
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HeaderIcon))]
        private bool _isCollapsed;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isVisible;

        [ObservableProperty]
        private string _title = string.Empty;

        public string HeaderIcon => IsCollapsed ? Icons.ArrowDown : Icons.ArrowUp;

        public bool IsNotBusy => !IsBusy;

        [ObservableProperty]
        private string _fullViewRoute = string.Empty;

        public abstract void LoadConfiguration();
        public abstract void SaveConfiguration();
    }
}