using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.AccountSetup
{
    public abstract partial class AccountSetupCarouselItemViewModelBase : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private string _title = string.Empty;

        public bool IsNotBusy => !IsBusy;

        public abstract Task ResetView();
        public abstract bool CanGoNext();
    }
}
