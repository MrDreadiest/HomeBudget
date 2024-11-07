namespace HomeBudget.App.Views.ContentViews.UniversalControls;

public partial class SelectBudgetContentView : ContentView
{
    private static readonly Random _random = new Random();

    public SelectBudgetContentView()
	{
        var red = _random.Next(0, 256);
        var green = _random.Next(0, 256);
        var blue = _random.Next(0, 256);

        InitializeComponent();

        this.IconFrame.Background = Color.FromRgb(red, green, blue);
    }
}