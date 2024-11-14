using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Main
{
    public abstract partial class MainCarouselItemViewModelBase : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private string _title = string.Empty;

        public bool IsNotBusy => !IsBusy;

        public abstract Task ResetView();
    }
}
