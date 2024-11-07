using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HomeBudget.App.ViewModels.ContentViewModels
{
    public partial class BaseContentViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task ToggleVisibility()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                IsVisible = !IsVisible;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        public BaseContentViewModel() { }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotCollapsed))]
        private bool isCollapsed;

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

        public bool IsNotCollapsed => !IsCollapsed;

        public bool IsNotBusy => !IsBusy;

        public bool IsNotVisible => !IsVisible;
    }
}
