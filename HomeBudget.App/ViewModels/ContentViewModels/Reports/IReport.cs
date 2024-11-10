namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public interface IReport
    {
        Task DataPresentation(Dictionary<string, Dictionary<string, decimal>> filteredData);
    }
}
