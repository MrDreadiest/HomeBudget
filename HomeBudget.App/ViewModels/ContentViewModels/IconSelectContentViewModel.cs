using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Resources.Icons;

namespace HomeBudget.App.ViewModels.ContentViewModels
{
    public partial class IconSelectContentViewModel : BaseViewModel
    {
        public event EventHandler? SelectedIconChanged;

        [RelayCommand]
        public async Task ToggleView()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                ToggleVisibility();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public void SelectIcon(IconItem iconItem)
        {
            if (iconItem.IsSelected)
            {
                if (!SelectedIconItem.Name.Equals(iconItem.Name))
                {
                    iconItem.IsSelected = false;
                    SelectedIconItem = null;
                    return;
                }
            }

            if (SelectedIconItem != null)
            {
                SelectedIconItem.IsSelected = false;
            }

            iconItem.IsSelected = true;
            SelectedIconItem = iconItem;
        }

        [ObservableProperty]
        private List<IconCategory> iconCategories;

        [ObservableProperty]
        private IconItem selectedIconItem;

        public bool IsPopulated => IconCategories.Count > 0;

        public IconSelectContentViewModel()
        {
            IsVisible = false;
            IconCategories = new List<IconCategory>();
            SelectedIconItem = new IconItem() { Name = "DEFAULT", Unicode = Icons.Budget };
        }

        public void ResetView()
        {
            IsVisible = false;

            if (IsPopulated)
            {
                SelectIcon(IconCategories.FirstOrDefault()!.Icons.FirstOrDefault()!);
            }
            else
            {
                SelectedIconItem = new IconItem() { Name = "DEFAULT", Unicode = Icons.Budget };
            }
        }

        public void ReloadData()
        {
            IconCategories.Clear();
            IconCategories = IconHelper.GetBudgetRelatedIcons()
            .Select(category => new IconCategory()
            {
                CategoryName = category.Key,
                Icons = category.Value
                .Select(icon => new IconItem()
                {
                    Name = icon.Name,
                    Unicode = icon.Unicode,
                    IsSelected = false
                }).ToList()
            }).ToList();

            SelectIcon(IconCategories.FirstOrDefault()!.Icons.FirstOrDefault()!);
        }

        public override Task OnAppearingAsync()
        {
            IsVisible = true;
            throw new NotImplementedException();
        }

        public override Task OnDisappearingAsync()
        {
            IsVisible = false;
            throw new NotImplementedException();
        }

        partial void OnSelectedIconItemChanged(IconItem value)
        {
            SelectedIconChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ToggleVisibility()
        {
            IsVisible = !IsVisible;
        }
    }
}
