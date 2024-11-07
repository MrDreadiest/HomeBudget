using HomeBudget.App.ViewModels.ContentViewModels.Reports;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;

namespace HomeBudget.App.Views.ContentViews.Reports;

public partial class AddReportGraphContentView : ContentView
{
    private AddReportGraphContentViewModel _viewModel;
    private CollapseManager _collapseManager;

    public AddReportGraphContentView() : this(App.Services.GetService<CollapseManager>()!)
    {
    }

    public AddReportGraphContentView(CollapseManager collapseManager)
    {
        _collapseManager = new CollapseManager();
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is AddReportGraphContentViewModel viewModel)
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