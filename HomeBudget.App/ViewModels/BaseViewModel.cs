using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.ViewModels.Interfaces;

namespace HomeBudget.App.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject, IViewDisappearing, IViewAppearing
    {
        public BaseViewModel() { }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotCollapsed))]
        private bool isCollapsed;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [NotifyPropertyChangedFor(nameof(IsNotVisible))]
        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private string route = string.Empty;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string iconUnicode = string.Empty;

        public bool IsNotCollapsed => !IsCollapsed;

        public bool IsNotBusy => !IsBusy;

        public bool IsNotVisible => !IsVisible;

        internal bool _isInitialized;

        public abstract Task OnAppearingAsync();
        public abstract Task OnDisappearingAsync();
    }
}