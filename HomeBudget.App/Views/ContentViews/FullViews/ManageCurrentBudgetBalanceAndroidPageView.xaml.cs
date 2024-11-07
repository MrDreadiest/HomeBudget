using HomeBudget.App.ViewModels.ContentViewModels.FullViews;

namespace HomeBudget.App.Views.ContentViews.FullViews;

public partial class ManageCurrentBudgetBalanceAndroidPageView : ContentPage
{
	private ManageCurrentBudgetBalancePageViewModel _viewModel;

	public ManageCurrentBudgetBalanceAndroidPageView(ManageCurrentBudgetBalancePageViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += ManageCurrentBudgetBalanceAndroidPageView_Appearing;
        Disappearing += ManageCurrentBudgetBalanceAndroidPageView_Disappearing;
        InitializeComponent();
	}

    private async void ManageCurrentBudgetBalanceAndroidPageView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void ManageCurrentBudgetBalanceAndroidPageView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}