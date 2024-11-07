using HomeBudget.App.ViewModels.ContentViewModels.FullViews;

namespace HomeBudget.App.Views.ContentViews.FullViews;

public partial class ManageCategoriesAndroidPageView : ContentPage
{
    private ManageTransactionCategoriesPageViewModel _viewModel;

    public ManageCategoriesAndroidPageView(ManageTransactionCategoriesPageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += ManageCategoriesAndroidPageView_Appearing;
        Disappearing += ManageCategoriesAndroidPageView_Disappearing;
        InitializeComponent();
    }

    private async void ManageCategoriesAndroidPageView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void ManageCategoriesAndroidPageView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }

    private void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {
        _viewModel.ToggleSwipeView(sender as SwipeView);
    }
}