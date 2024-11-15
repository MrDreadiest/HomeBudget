namespace HomeBudget.App.ViewModels.ContentViewModels.AccountSetup
{
    internal class AccountSetupCarouselItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserDT { get; set; }
        public DataTemplate BudgetDT { get; set; }
        public DataTemplate CategoriesDT { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is AccountSetupUserContentViewModel)
                return UserDT;
            else if (item is AccountSetupBudgetContentViewModel)
                return BudgetDT;
            else if (item is AccountSetupCategoriesContentViewModel)
                return CategoriesDT;
            return null;
        }
    }
}
