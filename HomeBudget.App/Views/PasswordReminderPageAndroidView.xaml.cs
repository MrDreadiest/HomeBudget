using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class PasswordReminderPageAndroidView : ContentPage
{
	PasswordReminderViewModel _viewModel;
	public PasswordReminderPageAndroidView(PasswordReminderViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;
        Disappearing += PasswordReminderPageAndroidView_Disappearing;
        Appearing += PasswordReminderPageAndroidView_Appearing;
		InitializeComponent();
	}

    private async void PasswordReminderPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }

    private async void PasswordReminderPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }
}