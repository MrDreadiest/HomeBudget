using HomeBudget.App.ViewModels.ContentViewModels.FullViews;

namespace HomeBudget.App.Views.ContentViews.FullViews;

public partial class ManageShortcutsAndroidPageView : ContentPage
{
	private ManageShortcutsPageViewModel _viewModel;

	public ManageShortcutsAndroidPageView(ManageShortcutsPageViewModel viewModel)
	{
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Appearing += ManageShortcutsAndroidPageView_Appearing;
        Disappearing += ManageShortcutsAndroidPageView_Disappearing;
        InitializeComponent();
	}

    private async void ManageShortcutsAndroidPageView_Disappearing(object? sender, EventArgs e)
    {
        await _viewModel.OnDisappearingAsync();
    }

    private async void ManageShortcutsAndroidPageView_Appearing(object? sender, EventArgs e)
    {
        await _viewModel.OnAppearingAsync();
    }
}