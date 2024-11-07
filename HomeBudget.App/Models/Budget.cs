
using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.Models
{
    public partial class Budget : ObservableObject
    {
        public required string Id { get; set; }
        public required string OwnerId { get; set; }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string iconUnicode = string.Empty;

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<TransactionCategory> TransactionCategories { get; set; } = new List<TransactionCategory>();
        public List<User> Users { get; private set; } = new List<User>();
    }
}
