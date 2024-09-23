using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.ViewModels.Interfaces;

namespace HomeBudget.App.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject, IViewDisappearing, IViewAppearing
    {
        public BaseViewModel() { }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;

        public abstract Task OnAppearingAsync();
        public abstract Task OnDisappearingAsync();
    }
}