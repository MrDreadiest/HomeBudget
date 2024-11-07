using HomeBudget.App.Services.Interfaces;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class SingleReportTableViewModel : SingleReportViewModelBase, ISingleReportSave
    {

        private readonly IAppSettingsService _appSettingsService;

        public SingleReportTableViewModel() : this(App.Services.GetService<IAppSettingsService>()!)
        {

        }

        public SingleReportTableViewModel(IAppSettingsService appSettingsService) : base(ReportType.Table)
        {
            _appSettingsService = appSettingsService;
        }


        public async Task LoadFromAppSettingsAsync()
        {
            await Task.CompletedTask;
        }

        public async Task SaveToAppSettingsAsync()
        {
            await Task.CompletedTask;
        }
    }
}
