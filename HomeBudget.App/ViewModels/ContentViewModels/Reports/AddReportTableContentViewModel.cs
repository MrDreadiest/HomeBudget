namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class AddReportTableContentViewModel : AddReportContentViewModelBase
    {

        public AddReportTableContentViewModel() : base(ReportType.Table)
        {
            IsCollapsed = true;
        }
    }
}
