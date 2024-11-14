using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public abstract partial class ReportCarouselItemBaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotCollapsed))]
        private bool _isCollapsed;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [NotifyPropertyChangedFor(nameof(IsNotVisible))]
        [ObservableProperty]
        private bool _isVisible;

        [ObservableProperty]
        private string _route = string.Empty;

        [ObservableProperty]
        private string _title = string.Empty;

        public bool IsNotCollapsed => !IsCollapsed;

        public bool IsNotBusy => !IsBusy;

        public bool IsNotVisible => !IsVisible;

        internal bool _isInitialized;

        public ReportCarouselItemBaseViewModel()
        {

        }
    }
}
