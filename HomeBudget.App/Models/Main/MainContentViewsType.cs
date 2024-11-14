namespace HomeBudget.App.Models.Main
{
    public enum MainContentViewsType
    {
        Login,
        Register
    }

    public static class MainContentViewsTypeExtensions
    {
        public static string GetDescription(this MainContentViewsType viewsType)
        {
            switch (viewsType)
            {
                case MainContentViewsType.Login:
                    return Resources.Languages.AppResource.MainContentViewsType_Login;
                case MainContentViewsType.Register:
                    return Resources.Languages.AppResource.MainContentViewsType_Register;
                default:
                    return string.Empty;
            }
        }
    }
}
