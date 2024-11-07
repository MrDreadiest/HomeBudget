using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.HandViews
{
    public partial class HandViewBaseModel : BaseViewModel
    {
        [ObservableProperty]
        private string fullViewRoute = string.Empty;

        public async override Task OnAppearingAsync()
        {
            IsVisible = true;
            await Task.CompletedTask;
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }
    }
}