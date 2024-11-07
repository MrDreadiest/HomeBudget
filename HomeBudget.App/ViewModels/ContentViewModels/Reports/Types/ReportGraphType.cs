using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public enum ReportGraphType
    {
        Time,
        Bar,
        Pie,
        Radial
    }

    public static class ReportGraphTypeExtensions
    {
        public static string GetDescription(this ReportGraphType graphType)
        {
            switch (graphType)
            {
                case ReportGraphType.Time:
                    return AppResource.ReportGraphType_Time;
                case ReportGraphType.Bar:
                    return AppResource.ReportGraphType_Bar;
                case ReportGraphType.Pie:
                    return AppResource.ReportGraphType_Pie;
                case ReportGraphType.Radial:
                    return AppResource.ReportGraphType_Radial;
                default:
                    return string.Empty;
            }
        }

        public static string GetImageSource(this ReportGraphType graphType)
        {
            switch (graphType)
            {
                case ReportGraphType.Time:
                    return "resources/images/time_chart.png";
                case ReportGraphType.Bar:
                    return "resources/images/bar_chart.png";
                case ReportGraphType.Pie:
                    return "resources/images/pie_chart.png";
                case ReportGraphType.Radial:
                    return "resources/images/radial_chart.png";
                default:
                    return string.Empty;
            }
        }
    }
}
