using HomeBudget.App.ViewModels.ContentViewModels.Reports;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;

namespace HomeBudget.App.Views.ContentViews.Reports;

public partial class AddReportTableContentView : ContentView
{
    private AddReportTableContentViewModel _viewModel;
    private CollapseManager _collapseManager;

    public AddReportTableContentView() : this(App.Services.GetService<CollapseManager>()!)
    {
    }

    public AddReportTableContentView(CollapseManager collapseManager)
    {
        _collapseManager = new CollapseManager();
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is AddReportTableContentViewModel viewModel)
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