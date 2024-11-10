using HomeBudget.App.ViewModels.ContentViewModels;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;
using System.ComponentModel;

namespace HomeBudget.App.Views.ContentViews;

public partial class IconSelectContentView : ContentView
{

    private IconSelectContentViewModel _viewModel;
    private FadeManager _fadeManager;

    public IconSelectContentView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;

        _fadeManager = new FadeManager(this);
    }


    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is IconSelectContentViewModel viewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _viewModel = viewModel;
            _fadeManager.Initialize(_viewModel.IsVisible);
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.IsVisible))
        {
            await _fadeManager.HandleCollapse(_viewModel.IsVisible);
        }
    }
}