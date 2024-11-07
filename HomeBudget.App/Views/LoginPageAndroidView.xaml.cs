using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class LoginPageAndroidView : ContentPage
{
	LoginPageViewModel _viewModel;
	public LoginPageAndroidView(LoginPageViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;
        Disappearing += LoginPageAndroidView_Disappearing;
        Appearing += LoginPageAndroidView_Appearing;
		InitializeComponent();
	}

    private async void LoginPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }

    private async void LoginPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }
}