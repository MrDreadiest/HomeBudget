using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.Models.Reports
{
    public enum ReportFilterType
    {
        Date,
        Category
    }

    public static class ReportFilterTypeExtensions
    {
        public static string GetDescription(this ReportFilterType filterType)
        {
            switch (filterType)
            {
                case ReportFilterType.Date:
                    return AppResource.ReportFilterType_Date;
                case ReportFilterType.Category:
                    return AppResource.ReportFilterType_Category;
                default:
                    return string.Empty;
            }
        }
    }
}
