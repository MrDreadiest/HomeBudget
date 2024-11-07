namespace HomeBudget.App.Models.Transactions
{
    public enum TransactionModifyActionType
    {
        Create,
        Update,
    }

    public static class TransactionModifyActionTypeExtensions
    {
        public static string GetDescription(this TransactionModifyActionType actionType)
        {
            switch (actionType)
            {
                case TransactionModifyActionType.Create:
                    return Resources.Languages.AppResource.TransactionModifyActionType_Create;
                case TransactionModifyActionType.Update:
                    return Resources.Languages.AppResource.TransactionModifyActionType_Update;
                default:
                    return string.Empty;
            }
        }
    }
}
