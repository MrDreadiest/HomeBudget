using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Filters
{
    public partial class TransactionPriceFilterContentViewModel : FilterContentViewModelBase, ITransactionFilter
    {
        [ObservableProperty]
        private decimal _minAmount;

        [ObservableProperty]
        private decimal _maxAmount;

        public TransactionPriceFilterContentViewModel()
        {
            // TODO: Przeniesienie do zasobów
            Title = "Kwota";
        }

        public IEnumerable<TransactionGroup> Apply(IEnumerable<TransactionGroup> transactions)
        {
            throw new NotImplementedException();
        }

        public async void Reply()
        {
            await Refilter();
        }
    }
}
