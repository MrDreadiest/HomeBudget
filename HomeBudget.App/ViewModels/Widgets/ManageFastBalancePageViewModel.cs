using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Models.TransactionCategories;
using HomeBudget.App.Models.WidgetConfigurations;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.Widgets
{
    public partial class ManageFastBalancePageViewModel : WidgetViewModelBase
    {

        [RelayCommand]
        private void Increment()
        {
            if (IsTopCounterAvailable)
            {
                if (TopCounter < CategoriesDropDownVM.TransactionCategories.Count)
                {
                    TopCounter++;
                }
            }
        }

        [RelayCommand]
        private void Decrement()
        {
            if (IsTopCounterAvailable)
            {
                if (TopCounter > FastBalanceWidgetConfiguration.TopCounterMin + 1)
                {
                    TopCounter--;
                }
            }
        }

        [ObservableProperty]
        private int _topCounter;

        [ObservableProperty]
        private bool _isShowPercentageSwitch;

        [ObservableProperty]
        private bool _isSumOtherAsLastSwitch;

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _selectedCategories = new();

        private TransactionCategoriesSelectType _transactionCategoriesSelectType;


        [ObservableProperty]
        private bool _isTopCounterAvailable;

        [ObservableProperty]
        public bool _isTopCounterNotAvailable;


        [ObservableProperty]
        private SelectableButtonGroupViewModel _categoriesSelectButtonGroupVM;

        [ObservableProperty]
        private DropdownTransactionCategoryContentViewModel _categoriesDropDownVM;

        private FastBalanceWidgetConfiguration _configuration;

        private readonly IConfigurationService _configurationService;

        public ManageFastBalancePageViewModel(IConfigurationService configurationService)
        {
            Title = "Szybki bilans";

            _configurationService = configurationService;
            _configuration = new FastBalanceWidgetConfiguration();

            CategoriesDropDownVM = new DropdownTransactionCategoryContentViewModel(SelectionMode.Multiple);
            CategoriesDropDownVM.SelectedTransactionCategoryChanged += CategoriesDropDownVM_SelectedTransactionCategoryChanged;


            CategoriesSelectButtonGroupVM = new SelectableButtonGroupViewModel(Enum.GetValues(typeof(TransactionCategoriesSelectType)).Cast<TransactionCategoriesSelectType>().Select(f => new OptionItem(f.GetDescription(), f)).ToList());
            CategoriesSelectButtonGroupVM.SelectedChanged += CategoriesSelectButtonGroupVM_SelectedChanged;
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await Task.Delay(100);

                await CategoriesDropDownVM.Reload();
                LoadConfiguration();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async override Task OnDisappearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                SaveConfiguration();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            IsVisible = false;
        }

        public override void LoadConfiguration()
        {
            _configuration = _configurationService.LoadConfiguration<FastBalanceWidgetConfiguration>();

            if (Enum.TryParse(_configuration.SelectType, out TransactionCategoriesSelectType selectType))
            {
                TopCounter = _configuration.TopCounter;
                IsShowPercentageSwitch = _configuration.IsShowPercentageSwitch;
                IsSumOtherAsLastSwitch = _configuration.IsSumOtherAsLastSwitch;

                CategoriesSelectButtonGroupVM.SelectOption(CategoriesSelectButtonGroupVM.Options.ToList().Find(o => (TransactionCategoriesSelectType)o.Type == selectType)!);

                CategoriesDropDownVM.SelectCategories(_configuration.SelectedCategoriesIds);
            }
        }

        public override void SaveConfiguration()
        {
            _configuration.SelectType = _transactionCategoriesSelectType.ToString();
            _configuration.SelectedCategoriesIds = SelectedCategories.Select(c => c.Id).ToList();
            _configuration.TopCounter = TopCounter;
            _configuration.IsShowPercentageSwitch = IsShowPercentageSwitch;
            _configuration.IsSumOtherAsLastSwitch = IsSumOtherAsLastSwitch;

            _configurationService.SaveConfiguration(_configuration);
        }

        private void CategoriesDropDownVM_SelectedTransactionCategoryChanged(object? sender, List<TransactionCategory> e)
        {
            SelectedCategories = new ObservableCollection<TransactionCategory>(e);
        }

        private void CategoriesSelectButtonGroupVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is TransactionCategoriesSelectType dateType)
                {
                    _transactionCategoriesSelectType = dateType;
                    IsTopCounterAvailable = dateType != TransactionCategoriesSelectType.Own;
                    IsTopCounterNotAvailable = !IsTopCounterAvailable;
                    CategoriesDropDownVM.IsEnable = IsTopCounterNotAvailable;

                    if (IsTopCounterAvailable)
                    {
                        CategoriesDropDownVM.ResetView();
                    }
                    else
                    {
                        TopCounter = FastBalanceWidgetConfiguration.TopCounterMin;
                    }
                }
            }
        }


    }
}
