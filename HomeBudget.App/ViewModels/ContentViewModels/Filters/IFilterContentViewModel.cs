
namespace HomeBudget.App.ViewModels.ContentViewModels.Filters
{
    public interface IFilterContentViewModel
    {
        Task ToggleIsCollapsed();
        Task ToggleVisibility();
        Task Filter();
    }
}
