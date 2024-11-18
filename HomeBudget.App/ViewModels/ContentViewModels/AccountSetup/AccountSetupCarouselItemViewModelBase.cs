using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.AccountSetup
{
    public abstract partial class AccountSetupCarouselItemViewModelBase : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotVisible))]
        private bool _isVisible;

        [ObservableProperty]
        private string _title = string.Empty;

        public bool IsNotBusy => !IsBusy;
        public bool IsNotVisible => !IsVisible;

        public IconSelectContentViewModel IconSelectVM { get; set; }

        public abstract Task ResetView();
        public abstract bool CanGoNext();

        public abstract void OnAppearing();
        public abstract void OnDisappearing();
    }
}
