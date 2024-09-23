using HomeBudget.App.ViewModels;

namespace HomeBudget.App
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel viewModel;
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            this.viewModel = mainPageViewModel;
            BindingContext = viewModel;
            InitializeComponent();
        }
    }

}
