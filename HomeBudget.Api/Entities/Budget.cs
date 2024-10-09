using HomeBudget.Common.EntityDTOs.Budget;
using System.Text.Json.Serialization;

namespace HomeBudget.Api.Entities
{
    public class Budget
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string OwnerId { get; private set; }

        #region ExpenseRelation
        [JsonIgnore]
        public ICollection<Expense> Expenses { get; private set; } = new List<Expense>();
        #endregion

        #region UserRelation
        [JsonIgnore]
        public List<User> Users { get; private set; } = new List<User>();
        #endregion

        #region ExpenseCategories
        [JsonIgnore]
        public List<ExpenseCategory> ExpenseCategories { get; private set; } = new List<ExpenseCategory>();
        #endregion

        private Budget()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Budget(User user, string name, string description) : this()
        {
            OwnerId = user.Id;
            Users.Add(user);
            user.Budgets.Add(this);

            Name = name;
            Description = description;
        }

        public BudgetCreateResponseModel ToBudgetCreateResponseModel()
        {
            return new BudgetCreateResponseModel()
            {
                Id = Id,
                Name = Name,
                Description = Description
            };
        }
    }
}
