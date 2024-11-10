using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.Models.Reports
{
    public enum ReportType
    {
        Table,
        Graph
    }

    public static class ReportTypeExtensions
    {
        public static string GetDescription(this ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.Table:
                    return AppResource.ReportType_Table;
                case ReportType.Graph:
                    return AppResource.ReportType_Graph;
                default:
                    return string.Empty;
            }
        }
    }
}
