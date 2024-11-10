using HomeBudget.App.ViewModels.ContentViewModels.Reports;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;

namespace HomeBudget.App.Views.ContentViews.Reports;

public partial class FilterReportContentView : ContentView
{
    private FilterReportContentViewModel _viewModel;
    private CollapseManager _collapseManager;

    public FilterReportContentView() : this(App.Services.GetService<CollapseManager>()!)
    {
    }

    public FilterReportContentView(CollapseManager collapseManager)
    {
        _collapseManager = collapseManager;
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is FilterReportContentViewModel viewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _viewModel = viewModel;
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