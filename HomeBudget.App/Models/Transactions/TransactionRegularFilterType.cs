namespace HomeBudget.App.Models.Transactions
{
    public enum TransactionRegularFilterType
    {
        RegularExpense = 1,
        RegularIncome = 3,
        All = 4,
    }

    public static class TransactionRegularFilterTypeExtensions
    {
        public static string GetDescription(this TransactionRegularFilterType transactionType)
        {
            switch (transactionType)
            {
                case TransactionRegularFilterType.RegularExpense:
                    return Resources.Languages.AppResource.TransactionFilterType_RegularExpense;
                case TransactionRegularFilterType.RegularIncome:
                    return Resources.Languages.AppResource.TransactionFilterType_RegularIncome;
                case TransactionRegularFilterType.All:
                    return Resources.Languages.AppResource.TransactionFilterType_All;
                default:
                    return string.Empty;
            }
        }
    }
}
