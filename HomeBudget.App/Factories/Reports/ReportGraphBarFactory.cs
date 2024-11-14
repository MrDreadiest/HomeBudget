using HomeBudget.App.ViewModels.ContentViewModels.Reports;

namespace HomeBudget.App.Factories.Reports
{
    public class ReportGraphBarFactory : IReportFactory
    {
        public IReport CreateReport(ReportViewModel reportViewModel)
        {
            return new ReportGraphBarViewModel(reportViewModel);
        }
    }
}
