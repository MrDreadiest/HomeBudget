using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class MainPageAndroidView : ContentPage
{
    MainPageViewModel _viewModel;
    public MainPageAndroidView(MainPageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Disappearing += MainPageAndroidView_Disappearing;
        Appearing += MainPageAndroidView_Appearing;
        InitializeComponent();
    }

    private async void MainPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }

    private async void MainPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }
}