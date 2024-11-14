using HomeBudget.App.ViewModels.ContentViewModels.Reports;

namespace HomeBudget.App.Factories.Reports
{
    public interface IReportFactory
    {
        IReport CreateReport(ReportViewModel reportViewModel);
    }
}
