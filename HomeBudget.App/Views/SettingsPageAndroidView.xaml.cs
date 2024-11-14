using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class SettingsPageAndroidView : ContentPage
{
    private SettingsPageViewModel _viewModel;

    public SettingsPageAndroidView(SettingsPageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += SettingsPageAndroidView_Appearing;
        Disappearing += SettingsPageAndroidView_Disappearing;

        InitializeComponent();
    }

    private async void SettingsPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void SettingsPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}