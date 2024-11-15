using HomeBudget.App.Resources.Icons;

namespace HomeBudget.App.Models.AccountSetup
{
    public enum AccountSetupContentViewsType
    {
        UserSetup,
        BudgetSetup,
        CategoriesSetup
    }

    public static class AccountSetupContentViewsTypeExtensions
    {
        public static string GetDescriptions(this AccountSetupContentViewsType viewsType)
        {
            return viewsType switch
            {
                AccountSetupContentViewsType.UserSetup => Resources.Languages.AppResource.AccountSetupContentViewsType_UserSetup,
                AccountSetupContentViewsType.BudgetSetup => Resources.Languages.AppResource.AccountSetupContentViewsType_BudgetSetup,
                AccountSetupContentViewsType.CategoriesSetup => Resources.Languages.AppResource.AccountSetupContentViewsType_CategoriesSetup,
                _ => string.Empty,
            };
        }
        public static string GetIcon(this AccountSetupContentViewsType viewsType)
        {
            return viewsType switch
            {
                AccountSetupContentViewsType.UserSetup => Icons.AccountCircle,
                AccountSetupContentViewsType.BudgetSetup => Icons.Budget,
                AccountSetupContentViewsType.CategoriesSetup => Icons.Categories,
                _ => string.Empty,
            };
        }
    }
}
