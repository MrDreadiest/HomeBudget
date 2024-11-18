using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Resources.Icons;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels
{
    public partial class IconSelectContentViewModel : BaseViewModel
    {
        public event EventHandler? SelectedIconChanged;

        [RelayCommand]
        public void ToggleView()
        {
            ToggleVisibility();
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
        private ObservableCollection<IconItem> _iconsCollection;

        [ObservableProperty]
        private IconItem selectedIconItem;

        public bool IsPopulated => IconCategories.Count > 0;

        public IconSelectContentViewModel()
        {
            IsVisible = false;
            IconCategories = new List<IconCategory>();
            SelectedIconItem = new IconItem() { Name = "DEFAULT", Unicode = Icons.Budget };

            IconsCollection = new ObservableCollection<IconItem>();
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
                    Category = category.Key,
                    Unicode = icon.Unicode,
                    IsSelected = false
                }).ToList()
            }).ToList();

            SelectIcon(IconCategories.FirstOrDefault()!.Icons.FirstOrDefault()!);

            foreach (var iconCategory in IconCategories)
            {
                foreach (var icon in iconCategory.Icons)
                {
                    IconsCollection.Add(icon);
                }
            }
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
