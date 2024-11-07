using HomeBudget.App.ViewModels.ContentViewModels.Shortcuts;

namespace HomeBudget.App.Views.ContentViews.Shortcuts;

public partial class ShortcutButtonContentView : ContentView
{
    ShortcutButtonContentViewModel _viewModel;

    public ShortcutButtonContentView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (BindingContext is ShortcutButtonContentViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}