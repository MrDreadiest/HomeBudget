using HomeBudget.App.ViewModels.ContentViewModels.FullViews;

namespace HomeBudget.App.Views.ContentViews.FullViews;

public partial class ManageCurrentBudgetAndroidPageView : ContentPage
{
	private ManageCurrentBudgetPageViewModel _viewModel;

	public ManageCurrentBudgetAndroidPageView(ManageCurrentBudgetPageViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += ManageCurrentBudgetAndroidPageView_Appearing;
        Disappearing += ManageCurrentBudgetAndroidPageView_Disappearing;
        InitializeComponent();
	}

    private async void ManageCurrentBudgetAndroidPageView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void ManageCurrentBudgetAndroidPageView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}