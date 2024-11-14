namespace HomeBudget.App.Views.ContentViews.Reports;

public partial class ReportTableContentView : ContentView
{
    private List<CollectionView> _innerCollectionViews = new();

    public ReportTableContentView()
    {
        InitializeComponent();
    }

    private void InnerCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {

    }
}