using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class ReportTableViewModel : ReportCarouselItemBaseViewModel
    {
        public AddReportContentViewModelBase AddReportTableVM { get; }

        public ObservableCollection<SingleReportViewModelBase> Reports { get; set; }

        public ReportType ReportType => ReportType.Table;

        public ReportTableViewModel()
        {
            Title = ReportType.GetDescription();

            Reports = new ObservableCollection<SingleReportViewModelBase>();

            AddReportTableVM = new AddReportTableContentViewModel();
            AddReportTableVM.AddCommandRaised += AddReportTableVM_AddCommandRaised;
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await Task.Delay(100);

                if (_isInitialized)
                {
                    await ReloadData();
                }
                else
                {
                    _isInitialized = true;
                }

                ResetView();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }

        private void ResetView()
        {

        }

        private async Task ReloadData()
        {

        }

        private void AddReportTableVM_AddCommandRaised(object? sender, EventArgs e)
        {
            if (sender is AddReportTableContentViewModel viewModel)
            {
                Reports.Add(new SingleReportTableViewModel());
            }
        }
    }
}