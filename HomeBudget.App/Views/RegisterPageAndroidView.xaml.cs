using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class RegisterPageAndroidView : ContentPage
{
	private RegisterPageViewModel _viewModel;

	public RegisterPageAndroidView(RegisterPageViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;
        Disappearing += RegisterPageAndroidView_Disappearing;
        Appearing += RegisterPageAndroidView_Appearing;
		InitializeComponent();
	}

    private async void RegisterPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }

    private async void RegisterPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }
}