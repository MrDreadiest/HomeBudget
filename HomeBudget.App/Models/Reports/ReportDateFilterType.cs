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

        public static (DateTime DateFrom, DateTime DateTo) GetDateRange(this ReportDateFilterType dateFilterType)
        {
            var today = DateTime.Today;

            var _dateFrom = DateTime.Now;
            var _dateTo = DateTime.Now;

            switch (dateFilterType)
            {
                case ReportDateFilterType.ThisMonth:
                    _dateFrom = new DateTime(today.Year, today.Month, 1);
                    _dateTo = _dateFrom.AddMonths(1).AddDays(-1);
                    break;

                case ReportDateFilterType.ThreeMonths:
                    _dateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-2);
                    _dateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;

                case ReportDateFilterType.SixMonths:
                    _dateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-5);
                    _dateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;

                case ReportDateFilterType.TwelveMonths:
                    _dateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-11);
                    _dateTo = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                    break;

                case ReportDateFilterType.Own:
                    _dateFrom = today;
                    _dateTo = today;
                    break;

                default:
                    break;
            }

            return (_dateFrom, _dateTo);
        }
    }
}
