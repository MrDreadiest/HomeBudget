using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public enum ReportSortType
    {
        None,
        Date,
        Category
    }

    public static class ReportSortExtensions
    {
        public static string GetDescription(this ReportSortType sortType)
        {
            switch (sortType)
            {
                case ReportSortType.Date:
                    return AppResource.ReportType_Table;
                case ReportSortType.Category:
                    return AppResource.ReportType_Graph;
                default:
                    return string.Empty;
            }
        }
    }
}
