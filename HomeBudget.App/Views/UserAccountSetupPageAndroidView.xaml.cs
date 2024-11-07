using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class UserAccountSetupPageAndroidView : ContentPage
{
    UserAccountSetupPageViewModel _viewModel;
	public UserAccountSetupPageAndroidView(UserAccountSetupPageViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Disappearing += UserAccountSetupPageAndroidView_Disappearing;
        Appearing += UserAccountSetupPageAndroidView_Appearing;
        InitializeComponent();
	}

    private async void UserAccountSetupPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }

    private async void UserAccountSetupPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }
}