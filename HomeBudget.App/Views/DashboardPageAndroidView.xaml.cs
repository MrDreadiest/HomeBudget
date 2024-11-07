using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class DashboardPageAndroidView : ContentPage
{
	private DashboardPageViewModel _viewModel;
	
    public DashboardPageAndroidView(DashboardPageViewModel viewModel)
	{
		_viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += DashboardPageAndroidView_Appearing;
        Disappearing += DashboardPageAndroidView_Disappearing;
		InitializeComponent();
	}

    private async void DashboardPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void DashboardPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}