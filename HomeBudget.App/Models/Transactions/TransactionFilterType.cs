namespace HomeBudget.App.Models.Transactions
{
    public enum TransactionFilterType
    {
        Expense = 0,
        Income = 2,
        All = 4,
    }

    public static class TransactionFilterTypeExtensions
    {
        public static string GetDescription(this TransactionFilterType transactionType)
        {
            switch (transactionType)
            {
                case TransactionFilterType.Expense:
                    return Resources.Languages.AppResource.TransactionFilterType_Expense;
                case TransactionFilterType.Income:
                    return Resources.Languages.AppResource.TransactionFilterType_Income;
                case TransactionFilterType.All:
                    return Resources.Languages.AppResource.TransactionFilterType_All;
                default:
                    return string.Empty;
            }
        }
    }
}
