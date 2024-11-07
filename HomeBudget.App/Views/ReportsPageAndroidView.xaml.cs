using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class ReportsPageAndroidView : ContentPage
{
	private ReportsPageViewModel _viewModel;

	public ReportsPageAndroidView(ReportsPageViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += ReportsPageAndroidView_Appearing;
        Disappearing += ReportsPageAndroidView_Disappearing;

        InitializeComponent();
	}

    private async void ReportsPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void ReportsPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}