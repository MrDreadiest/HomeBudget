namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public interface ISingleReportSave
    {
        Task LoadFromAppSettingsAsync();
        Task SaveToAppSettingsAsync();
    }
}
