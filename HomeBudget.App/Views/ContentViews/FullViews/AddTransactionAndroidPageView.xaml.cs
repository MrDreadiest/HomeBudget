using HomeBudget.App.ViewModels.ContentViewModels.FullViews;

namespace HomeBudget.App.Views.ContentViews.FullViews;

public partial class AddTransactionAndroidPageView : ContentPage
{
    private ManageTransactionsPageViewModel _viewModel;

    public AddTransactionAndroidPageView(ManageTransactionsPageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += AddTransactionAndroidPageView_Appearing;
        Disappearing += AddTransactionAndroidPageView_Disappearing;
        InitializeComponent();
    }

    private async void AddTransactionAndroidPageView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void AddTransactionAndroidPageView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}