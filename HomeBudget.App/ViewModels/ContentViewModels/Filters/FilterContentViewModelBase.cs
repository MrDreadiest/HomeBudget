using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HomeBudget.App.ViewModels.ContentViewModels.Filters
{
    public partial class FilterContentViewModelBase : BaseViewModel, IFilterContentViewModel
    {
        public event EventHandler? FilterViewCall;

        [RelayCommand]
        public async Task ToggleIsCollapsed()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                IsCollapsed = !IsCollapsed;
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

        [RelayCommand]
        public async Task Filter()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                IsActive = !IsActive;
                FilterViewCall?.Invoke(this, EventArgs.Empty);
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

        [RelayCommand]
        public async Task Refilter()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                if (IsActive)
                {
                    FilterViewCall?.Invoke(this, EventArgs.Empty);
                }
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

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotActive))]
        [NotifyPropertyChangedFor(nameof(ActionName))]
        private bool _isActive;

        public bool IsNotActive => !IsActive;

        //TODO : Do zasobów
        public string ActionName => IsActive ? "Wyłącz filtr" : "Włącz filtr";
    }
}
