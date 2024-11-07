namespace HomeBudget.App.ViewModels.ContentViewModels.Shortcuts
{
    public enum ShortcutType
    {
        SplitTransaction,
        EditTransaction,
        AddTransaction,
        ManageCategory,
    }

    public static class ShortcutTypeExtensions
    {
        public static string GetDescription(this ShortcutType transactionType)
        {
            switch (transactionType)
            {
                case ShortcutType.AddTransaction:
                    return Resources.Languages.AppResource.ShortcutType_AddTransaction;
                case ShortcutType.EditTransaction:
                    return Resources.Languages.AppResource.ShortcutType_EditTransaction;
                case ShortcutType.SplitTransaction:
                    return Resources.Languages.AppResource.ShortcutType_SplitTransaction;
                case ShortcutType.ManageCategory:
                    return Resources.Languages.AppResource.ShortcutType_ManageCategory;
                default:
                    return string.Empty;
            }
        }
    }
}
