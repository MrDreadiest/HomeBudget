namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper
{
    public interface ICollapseContentViewModel
    {
        void ToggleIsCollapsed();
        bool IsCollapsed { get; set; }
        string Title { get; set; }
    }
}
