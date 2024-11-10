using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.Models.Reports
{
    public enum ReportSortType
    {
        Price,
        MostUse
    }

    public static class ReportSortTypeExtensions
    {
        public static string GetDescription(this ReportSortType sortType)
        {
            switch (sortType)
            {
                case ReportSortType.Price:
                    return AppResource.ReportSortType_MostUse;
                case ReportSortType.MostUse:
                    return AppResource.ReportSortType_Price;
                default:
                    return string.Empty;
            }
        }
    }
}
