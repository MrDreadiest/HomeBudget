using HomeBudget.App.ViewModels.ContentViewModels.FullViews;

namespace HomeBudget.App.Views.ContentViews.FullViews;

public partial class ManageRegularTransactionsAndroidPageView : ContentPage
{
	private ManageRegularTransactionsPageViewModel _viewModel;

	public ManageRegularTransactionsAndroidPageView(ManageRegularTransactionsPageViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += ManageRegularTransactionsAndroidPageView_Appearing;
        Disappearing += ManageRegularTransactionsAndroidPageView_Disappearing;
        InitializeComponent();
	}

    private async void ManageRegularTransactionsAndroidPageView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void ManageRegularTransactionsAndroidPageView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}