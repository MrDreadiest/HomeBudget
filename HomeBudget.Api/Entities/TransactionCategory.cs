using System.Text.Json.Serialization;

namespace HomeBudget.Api.Entities
{
    public class TransactionCategory
    {
        public string Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string IconUnicode { get; set; } = string.Empty;

        #region TransactionRelation
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
        #endregion

        #region BudgetRelation
        [JsonIgnore]
        public string BudgetId { get; set; } = null!;
        [JsonIgnore]
        public Budget Budget { get; set; } = null!;
        #endregion

        private TransactionCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public TransactionCategory(Budget budget) : this()
        {
            Budget = budget;
            BudgetId = budget.Id;
            Budget.TransactionCategories.Add(this);
        }

        public TransactionCategory(Budget budget, string name, string iconUnicode) : this(budget)
        {
            Name = name;
            IconUnicode = iconUnicode;
        }
    }
}
