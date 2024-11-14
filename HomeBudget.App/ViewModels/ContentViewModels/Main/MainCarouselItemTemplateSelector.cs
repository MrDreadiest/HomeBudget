namespace HomeBudget.App.ViewModels.ContentViewModels.Main
{
    internal class MainCarouselItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LoginDT { get; set; }
        public DataTemplate RegisterDT { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is LoginContentViewModel)
                return LoginDT;
            else if (item is RegisterContentViewModel)
                return RegisterDT;
            return null;
        }
    }
}
