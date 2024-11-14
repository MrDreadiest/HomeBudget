using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;
using HomeBudget.App.ViewModels.Widgets;

namespace HomeBudget.App.Views.Widgets;

public partial class FastBalanceContentView : ContentView
{
    private CollapseManager _collapseManager;
    private FastBalanceContentViewModel _viewModel;

    public FastBalanceContentView() : this(App.Services.GetService<CollapseManager>()!)
    {
    }

    public FastBalanceContentView(CollapseManager collapseManager)
    {
        _collapseManager = collapseManager;
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is FastBalanceContentViewModel viewModel)
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