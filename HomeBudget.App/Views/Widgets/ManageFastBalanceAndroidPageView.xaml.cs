using HomeBudget.App.ViewModels.Widgets;

namespace HomeBudget.App.Views.Widgets;

public partial class ManageFastBalanceAndroidPageView : ContentPage
{
    private ManageFastBalancePageViewModel _viewModel;

    public ManageFastBalanceAndroidPageView(ManageFastBalancePageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += ManageFastBalanceAndroidPageView_Appearing;
        Disappearing += ManageFastBalanceAndroidPageView_Disappearing;
        InitializeComponent();
    }

    private async void ManageFastBalanceAndroidPageView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void ManageFastBalanceAndroidPageView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}