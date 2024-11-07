using CommunityToolkit.Mvvm.ComponentModel;

namespace HomeBudget.App.Models
{
    public partial class TransactionCategory : ObservableObject
    {
        public required string Id { get; set; }
        public required string BudgetId { get; set; }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string iconUnicode = string.Empty;
    }
}
