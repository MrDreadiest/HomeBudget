namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class ReportGraphViewModel : ReportCarouselItemBaseViewModel
    {
        public AddReportGraphContentViewModel AddReportGraphVM { get; }


        public ReportType ReportType => ReportType.Graph;

        public ReportGraphViewModel()
        {
            Title = ReportType.GetDescription();

            AddReportGraphVM = new AddReportGraphContentViewModel();
            AddReportGraphVM.AddCommandRaised += AddReportGraphVM_AddCommandRaised;
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

        private void AddReportGraphVM_AddCommandRaised(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
