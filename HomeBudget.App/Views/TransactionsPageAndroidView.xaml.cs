using HomeBudget.App.ViewModels;

namespace HomeBudget.App.Views;

public partial class TransactionsPageAndroidView : ContentPage
{
    private TransactionsPageViewModel _viewModel;

    public TransactionsPageAndroidView(TransactionsPageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += TransactionsPageAndroidView_Appearing;
        Disappearing += TransactionsPageAndroidView_Disappearing;

        InitializeComponent();
    }

    private async void TransactionsPageAndroidView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void TransactionsPageAndroidView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }

}