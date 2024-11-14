using HomeBudget.App.ViewModels.ContentViewModels.Reports;

namespace HomeBudget.App.Factories.Reports
{
    public class ReportGraphPieFactory : IReportFactory
    {
        public IReport CreateReport(ReportViewModel reportViewModel)
        {
            return new ReportGraphPieViewModel(reportViewModel);
        }
    }
}
