using HomeBudget.Common.EntityDTOs.ExpenseCategory;
using System.Text.Json.Serialization;

namespace HomeBudget.Api.Entities
{
    public class ExpenseCategory
    {
        public string Id { get; private set; }
        public string Name { get; set; } = string.Empty;

        #region ExpenseRelation
        public ICollection<Expense> Expenses { get; private set; } = new List<Expense>();
        #endregion

        #region BudgetRelation
        [JsonIgnore]
        public string BudgetId { get; set; } = null!;
        [JsonIgnore]
        public Budget Budget { get; set; } = null!;
        #endregion

        private ExpenseCategory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ExpenseCategory(string name, Budget budget) : this()
        {
            Name = name;
            Budget = budget;
            BudgetId = budget.Id;
            Budget.ExpenseCategories.Add(this);
        }

    }
}
