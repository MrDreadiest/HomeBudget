namespace HomeBudget.App.ViewModels.ContentViewModels.Filters
{
    public interface ITransactionFilter
    {
        event EventHandler? FilterViewCall;

        IEnumerable<TransactionGroup> Apply(IEnumerable<TransactionGroup> transactions);
        void Reply();
        bool IsActive { get; set; }
        bool IsNotActive { get; }
    }
}
