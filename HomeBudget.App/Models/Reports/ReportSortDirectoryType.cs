using HomeBudget.App.Resources.Languages;

namespace HomeBudget.App.Models.Reports
{
    public enum ReportSortDirectoryType
    {
        Asc,
        Desc
    }

    public static class ReportSortDirectoryTypeExtensions
    {
        public static string GetDescription(this ReportSortDirectoryType sortDirectionType)
        {
            switch (sortDirectionType)
            {
                case ReportSortDirectoryType.Asc:
                    return AppResource.ReportSortDirectoryType_Asc;
                case ReportSortDirectoryType.Desc:
                    return AppResource.ReportSortDirectoryType_Desc;
                default:
                    return string.Empty;
            }
        }
    }
}
