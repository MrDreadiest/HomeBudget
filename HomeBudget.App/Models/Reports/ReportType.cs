using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.Models.Reports
{
    public enum ReportType
    {
        Table,
        GraphBar,
        GraphPie
    }

    public static class ReportTypeExtensions
    {
        public static string GetDescription(this ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.Table:
                    return AppResource.ReportType_Table;
                case ReportType.GraphBar:
                    return AppResource.ReportType_GraphBar;
                case ReportType.GraphPie:
                    return AppResource.ReportType_GraphPie;
                default:
                    return string.Empty;
            }
        }
    }
}
