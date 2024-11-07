namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    internal class ReportCarouselItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ReportTableDT { get; set; }
        public DataTemplate ReportGraphDT { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ReportTableViewModel)
                return ReportTableDT;
            else if (item is ReportGraphViewModel)
                return ReportGraphDT;

            return null;
        }
    }
}
