using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class AddReportContentViewModelBase : ObservableObject
    {
        public event EventHandler? AddCommandRaised;

        [RelayCommand]
        public void Add()
        {
            if (IsValid())
            {
                AddCommandRaised?.Invoke(this, EventArgs.Empty);
            }
        }

        [RelayCommand]
        public async Task ToggleIsCollapsed()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                IsCollapsed = !IsCollapsed;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        public AddReportContentViewModelBase(ReportType reportType)
        {
            ReportType = reportType;
            Title = String.Concat("Dodaj", " ", ReportType.GetDescription());
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotCollapsed))]
        private bool isCollapsed;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        public bool IsNotCollapsed => !IsCollapsed;

        public bool IsNotBusy => !IsBusy;

        public ReportType ReportType { get; }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
                return false;

            return true;
        }
    }
}
