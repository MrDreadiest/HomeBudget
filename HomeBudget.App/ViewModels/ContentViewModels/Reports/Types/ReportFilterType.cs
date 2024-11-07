using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public enum ReportFilterType
    {
        None,
        Asc,
        Desc,
        MostUse,
    }

    public static class ReportFilterExtensions
    {
        public static string GetDescription(this ReportFilterType filterType)
        {
            switch (filterType)
            {
                case ReportFilterType.Asc:
                    return AppResource.ReportType_Table;
                case ReportFilterType.Desc:
                    return AppResource.ReportType_Graph;
                case ReportFilterType.None:
                    return AppResource.ReportType_Graph;
                case ReportFilterType.MostUse:
                    return AppResource.ReportType_Graph;
                default:
                    return string.Empty;
            }
        }
    }
}
