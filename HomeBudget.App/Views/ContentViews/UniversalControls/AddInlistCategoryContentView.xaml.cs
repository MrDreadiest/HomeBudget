using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;

namespace HomeBudget.App.Views.ContentViews.UniversalControls;

public partial class AddInlistCategoryContentView : ContentView
{
    private AddInlistCategoryContentViewModel _viewModel;
    private CollapseManager _collapseManager;

    public AddInlistCategoryContentView() : this(App.Services.GetService<CollapseManager>()!)
    {
    }

    public AddInlistCategoryContentView(CollapseManager collapseManager)
    {
        _collapseManager = collapseManager;

        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;

        IsVisible = false;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is AddInlistCategoryContentViewModel viewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _viewModel = viewModel;
            IsVisible = true;
            _collapseManager.Initialize(this, _viewModel.IsCollapsed);
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private async void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.IsCollapsed))
        {
            await _collapseManager.HandleCollapse(_viewModel.IsCollapsed);
        }
    }
}