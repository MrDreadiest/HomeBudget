using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.Models.Reports
{
    public enum ReportDateFilterType
    {
        ThisMonth,
        ThreeMonths,
        SixMonths,
        TwelveMonths,
        //ThisYear,
        Own,
    }

    public static class ReportDateFilterTypeExtensions
    {
        public static string GetDescription(this ReportDateFilterType filterType)
        {
            switch (filterType)
            {
                case ReportDateFilterType.ThisMonth:
                    return AppResource.ReportDateFilterType_ThisMonth;
                case ReportDateFilterType.ThreeMonths:
                    return AppResource.ReportDateFilterType_ThreeMonths;
                case ReportDateFilterType.SixMonths:
                    return AppResource.ReportDateFilterType_SixMonths;
                case ReportDateFilterType.TwelveMonths:
                    return AppResource.ReportDateFilterType_TwelveMonths;
                //case ReportDateFilterType.ThisYear:
                //    return AppResource.ReportDateFilterType_ThisYear;
                case ReportDateFilterType.Own:
                    return AppResource.ReportDateFilterType_Own;
                default:
                    return string.Empty;
            }
        }
    }
}
