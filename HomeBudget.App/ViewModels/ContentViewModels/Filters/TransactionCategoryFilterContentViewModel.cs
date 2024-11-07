
namespace HomeBudget.App.ViewModels.ContentViewModels.Filters
{
    public partial class TransactionCategoryFilterContentViewModel : FilterContentViewModelBase, ITransactionFilter
    {
        public TransactionCategoryFilterContentViewModel()
        {
            // TODO: Przeniesienie do zasobów
            Title = "Kategoria";
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
