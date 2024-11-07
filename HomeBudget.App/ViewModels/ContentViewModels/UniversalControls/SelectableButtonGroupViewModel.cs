using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls
{
    public partial class SelectableButtonGroupViewModel : ObservableObject
    {
        public event EventHandler? SelectedChanged;

        [RelayCommand]
        public void SelectOption(OptionItem option)
        {
            SelectedOption = option;
            SelectedChanged?.Invoke(this, EventArgs.Empty);
            foreach (var opt in Options)
            {
                opt.IsSelected = opt == option;
            }
        }

        [ObservableProperty]
        private ObservableCollection<OptionItem> _options;

        [ObservableProperty]
        private OptionItem _selectedOption;

        public Enum? SelectedType => SelectedOption?.Type;

        public SelectableButtonGroupViewModel(List<OptionItem> optionItems) : base()
        {
            Options = new ObservableCollection<OptionItem>(optionItems);
            SelectedOption = Options[0];
            SelectOption(SelectedOption);
        }

        public void SelectWithoutNotify(int index)
        {
            var option = Options[index];

            SelectedOption = option;
            foreach (var opt in Options)
            {
                opt.IsSelected = opt == option;
            }
        }
    }

    public partial class OptionItem : ObservableObject
    {
        [ObservableProperty]
        private string _text = string.Empty;

        [ObservableProperty]
        private Enum _type;

        [ObservableProperty]
        private bool _isSelected;

        public OptionItem(string text, Enum type)
        {
            Text = text;
            Type = type;
        }
    }
}
