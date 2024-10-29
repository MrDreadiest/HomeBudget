
namespace HomeBudget.Api.Entities
{
    public class Transaction
    {
        public string Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public string CreatorId { get; private set; }

        #region CategoryRelation
        public string TransactionCategoryId { get; private set; } = null!;
        public TransactionCategory TransactionCategory { get; set; } = null!;
        #endregion

        #region BudgetRelation
        public string BudgetId { get; private set; } = null!;
        public Budget Budget { get; set; } = null!;
        #endregion

        private Transaction()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Transaction(Budget budget, TransactionCategory transactionCategory, string userId) : this()
        {
            Budget = budget;
            BudgetId = budget.Id;
            Budget.Transactions.Add(this);

            TransactionCategory = transactionCategory;
            TransactionCategoryId = transactionCategory.Id;
            TransactionCategory.Transactions.Add(this);

            CreatorId = userId;
        }
    }
}
