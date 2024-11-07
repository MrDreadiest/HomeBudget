using HomeBudget.App.ViewModels.ContentViewModels;

namespace HomeBudget.App.Views.ContentViews;

public partial class TransactionListContentView : ContentView
{
    private TransactionListContentViewModel _viewModel;

    public TransactionListContentView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is TransactionListContentViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }

    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        var scrollY = e.ScrollY;

        double cumulativeHeight = 0;

        foreach (var group in _viewModel.GroupedTransactions)
        {
            double groupHeaderHeight = 40;
            double itemHeight = 68;
            double groupHeight = groupHeaderHeight + (group.Count * itemHeight);

            cumulativeHeight += groupHeight;

            if (scrollY < cumulativeHeight)
            {
                string newGroupTitle = group.Date.ToString("dd MMMM yyyy");
                if (_viewModel.CurrentGroupTitle != newGroupTitle)
                {
                    _viewModel.CurrentGroupTitle = newGroupTitle;
                }
                break;
            }
        }
    }

    private void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {
        _viewModel.ToggleSwipeView(sender as SwipeView);
    }
}