namespace HomeBudget.App.Models.TransactionCategories
{
    public enum TransactionCategoryModifyActionType
    {
        Create,
        Update,
    }

    public static class TransactionCategoryModifyActionTypeExtensions
    {
        public static string GetDescription(this TransactionCategoryModifyActionType actionType)
        {
            switch (actionType)
            {
                case TransactionCategoryModifyActionType.Create:
                    return Resources.Languages.AppResource.TransactionModifyActionType_Create;
                case TransactionCategoryModifyActionType.Update:
                    return Resources.Languages.AppResource.TransactionModifyActionType_Update;
                default:
                    return string.Empty;
            }
        }
    }
}