using HomeBudget.App.ViewModels.ContentViewModels.Filters;

namespace HomeBudget.App.Views.ContentViews.Filters;

public partial class TransactionDateFilterContentView : ContentView
{
    FilterContentViewModelBase _viewModel;

    public TransactionDateFilterContentView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;

        TranslationY = 500;
        IsVisible = false;
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is FilterContentViewModelBase viewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _viewModel = viewModel;
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private async void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewModel.IsVisible))
        {
            if (_viewModel.IsVisible)
            {
                IsVisible = _viewModel.IsVisible;
                await ShowFilterAsync();
            }
            else
            {
                await HideFilterAsync();
                IsVisible = _viewModel.IsVisible;
            }
        }
    }

    private async Task ShowFilterAsync()
    {
        await this.TranslateTo(0, 0, 500, Easing.Linear);
    }

    private async Task HideFilterAsync()
    {
        await this.TranslateTo(0, 500, 500, Easing.Linear);
    }
}