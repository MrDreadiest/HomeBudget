
namespace HomeBudget.Api.Entities
{
    public class Expense
    {
        public string Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime Date {  get; set; } = DateTime.Now;

        public string CreatorId { get; private set; }

        #region CategoryRelation
        public string ExpenseCategoryId { get; set; } = null!;
        public ExpenseCategory ExpenseCategory { get; set; } = null!;
        #endregion

        #region BudgetRelation
        public string BudgetId { get; set; } = null!;
        public Budget Budget { get; set; } = null!;
        #endregion

        private Expense()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Expense(User user, Budget budget, ExpenseCategory expenseCategory) : this()
        {
            Budget = budget;
            BudgetId = budget.Id;
            Budget.Expenses.Add(this);

            ExpenseCategory = expenseCategory;
            ExpenseCategoryId = expenseCategory.Id;
            ExpenseCategory.Expenses.Add(this);

            CreatorId = user.Id;
        }
    }
}
