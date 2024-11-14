using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.ViewModels.Interfaces;
using HomeBudget.App.Views;

namespace HomeBudget.App.ViewModels.Widgets
{
    public abstract partial class WidgetViewModelBase : ObservableObject, IViewDisappearing, IViewAppearing, IConfigurable
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isVisible;

        [ObservableProperty]
        private string _title = string.Empty;

        public bool IsNotBusy => !IsBusy;

        [RelayCommand]
        public async Task Back()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"//{nameof(SettingsPageAndroidView)}", false);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
                IsVisible = false;
            }
            await Task.CompletedTask;
        }

        public abstract void LoadConfiguration();
        public abstract void SaveConfiguration();

        public abstract Task OnAppearingAsync();
        public abstract Task OnDisappearingAsync();
    }
}
