using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class BudgetsPageAndroidView : ContentPage
{
	private BudgetsPageViewModel _viewModel;

	public BudgetsPageAndroidView(BudgetsPageViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += BudgetsPageAndroidView_Appearing;
        Disappearing += BudgetsPageAndroidView_Disappearing;

        InitializeComponent();
	}

    private async void BudgetsPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void BudgetsPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}