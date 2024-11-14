using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Models.Main;
using HomeBudget.App.ViewModels.ContentViewModels.Main;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<MainCarouselItemViewModelBase> MainCarouselVMs { get; }

        [ObservableProperty]
        private int _carouselPosition;

        public SelectableButtonGroupViewModel SelectableButtonGroupVM { get; }

        public MainPageViewModel()
        {
            var registerVM = new RegisterContentViewModel();
            registerVM.OnRegisterContent += RegisterVM_OnRegisterContent; ;

            MainCarouselVMs = new ObservableCollection<MainCarouselItemViewModelBase>()
            {
                new LoginContentViewModel(),
                registerVM
            };

            SelectableButtonGroupVM = new SelectableButtonGroupViewModel(
                new List<OptionItem> {
                    new OptionItem(MainContentViewsType.Login.GetDescription(),MainContentViewsType.Login),
                    new OptionItem(MainContentViewsType.Register.GetDescription(),MainContentViewsType.Register),
                });
            SelectableButtonGroupVM.SelectedChanged += SelectableButtonGroupVM_SelectedChanged; ;
        }

        private void RegisterVM_OnRegisterContent(object? sender, (string email, string password) e)
        {
            SelectableButtonGroupVM.SelectWithNotify(0);
        }

        private void SelectableButtonGroupVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is MainContentViewsType reportType)
                {
                    int targetPosition = reportType switch
                    {
                        MainContentViewsType.Login => 0,
                        MainContentViewsType.Register => 1,
                        _ => CarouselPosition
                    };
                    CarouselPosition = targetPosition;
                }
            }
        }

        public async override Task OnAppearingAsync()
        {
            IsVisible = true;
            await Task.Delay(200);

            foreach (var itemVM in MainCarouselVMs)
            {
                _ = Task.Run(() => { itemVM.ResetView(); });
            }
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
        }
    }
}
