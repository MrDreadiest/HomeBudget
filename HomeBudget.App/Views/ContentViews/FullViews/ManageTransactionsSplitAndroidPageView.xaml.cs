using HomeBudget.App.ViewModels.ContentViewModels.FullViews;

namespace HomeBudget.App.Views.ContentViews.FullViews;

public partial class ManageTransactionsSplitAndroidPageView : ContentPage
{
    private AddTransactionBySplitPageViewModel _viewModel;

    public ManageTransactionsSplitAndroidPageView(AddTransactionBySplitPageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += ManageTransactionsSplitAndroidPageView_Appearing;
        Disappearing += ManageTransactionsSplitAndroidPageView_Disappearing;
        InitializeComponent();
    }

    private async void ManageTransactionsSplitAndroidPageView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void ManageTransactionsSplitAndroidPageView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }

    private void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {
        _viewModel.ToggleSwipeView(sender as SwipeView);
    }
}