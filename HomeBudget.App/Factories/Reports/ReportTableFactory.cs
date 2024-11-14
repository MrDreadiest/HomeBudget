using HomeBudget.App.ViewModels.ContentViewModels.Reports;

namespace HomeBudget.App.Factories.Reports
{
    public class ReportTableFactory : IReportFactory
    {
        public IReport CreateReport(ReportViewModel reportViewModel)
        {
            return new ReportTableViewModel(reportViewModel);
        }
    }
}
