using System.Text.Json.Serialization;

namespace HomeBudget.Api.Entities
{
    public class Budget
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUnicode { get; set; } = string.Empty;

        public string OwnerId { get; private set; }

        #region TransactionRelation
        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
        #endregion

        #region UserRelation
        [JsonIgnore]
        public List<User> Users { get; private set; } = new List<User>();
        #endregion

        #region TransactionCategories
        [JsonIgnore]
        public List<TransactionCategory> TransactionCategories { get; private set; } = new List<TransactionCategory>();
        #endregion

        private Budget()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Budget(User user) : this() 
        {
            OwnerId = user.Id;
            Users.Add(user);
            user.Budgets.Add(this);
        }

        public Budget(User user, string name, string description, string iconUnicode) : this(user)
        {
            Name = name;
            Description = description;
            IconUnicode = iconUnicode;
        }
    }
}
