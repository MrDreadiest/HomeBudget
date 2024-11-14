namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    internal class ReportCarouselItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ReportTableDT { get; set; }
        public DataTemplate ReportGraphPieDT { get; set; }
        public DataTemplate ReportGraphBarDT { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ReportTableViewModel)
                return ReportTableDT;
            else if (item is ReportGraphBarViewModel)
                return ReportGraphBarDT;
            else if (item is ReportGraphPieViewModel)
                return ReportGraphPieDT;
            return null;
        }
    }
}
