using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;

namespace HomeBudget.App.ViewModels.ContentViewModels.Filters
{
    public partial class TransactionTypeFilterContentViewModel : FilterContentViewModelBase, ITransactionFilter
    {
        [ObservableProperty]
        private SelectableButtonGroupViewModel selectableButtonGroupVM;

        public TransactionTypeFilterContentViewModel()
        {
            // TODO: Przeniesienie do zasobów
            Title = "Rodzaj";
            SelectableButtonGroupVM = new SelectableButtonGroupViewModel(
                new List<OptionItem> {
                    new OptionItem("Wszystkie",TransactionType.All),
                    new OptionItem("Przychód",TransactionType.Income),
                    new OptionItem("Wydatek",TransactionType.Expense),
                });
        }

        public IEnumerable<TransactionGroup> Apply(IEnumerable<TransactionGroup> transactions)
        {
            throw new NotImplementedException();
        }

        public async void Reply()
        {
            await Refilter();
        }

        public enum TransactionType
        {
            All,
            Income,
            Expense
        }
    }
}
