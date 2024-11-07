using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HomeBudget.App.ViewModels.ContentViewModels.Shortcuts
{
    public abstract partial class ShortcutBaseContentViewModel : ObservableObject
    {
        public ShortcutBaseContentViewModel()
        {

        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [NotifyPropertyChangedFor(nameof(IsNotVisible))]
        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private string route = string.Empty;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string iconUnicode = string.Empty;

        public bool IsNotBusy => !IsBusy;

        public bool IsNotVisible => !IsVisible;

        [RelayCommand]
        public async Task GoToAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"//{Route}");
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }
    }
}
