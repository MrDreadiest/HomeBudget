using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls
{
    public partial class StateIconIndicatorContentViewModel : ObservableObject
    {
        public event EventHandler? ProcessStateChanged;

        [ObservableProperty]
        private ObservableCollection<StateIconItem> _states;

        [ObservableProperty]
        private StateIconItem _currentState;

        private readonly bool _indicatePassed;

        public StateIconIndicatorContentViewModel(List<StateIconItem> stateItems, bool indicatePassed = false)
        {
            _indicatePassed = indicatePassed;
            States = new ObservableCollection<StateIconItem>(stateItems);
            CurrentState = States.First();
        }

        public void SelectState(int index, bool notify = true)
        {
            Select(States[index], notify);
        }

        public void SelectState(Enum type, bool notify = true)
        {
            Select(States.ToList().Find(state => state.Type == type), notify);
        }

        private void Select(StateIconItem state, bool notify = true)
        {
            if (state != null)
            {
                CurrentState = state;

                if (notify)
                {
                    ProcessStateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        partial void OnCurrentStateChanged(StateIconItem? oldValue, StateIconItem newValue)
        {
            if (!_indicatePassed)
            {
                if (oldValue != null)
                {
                    oldValue.IsPassed = false;
                }
            }

            newValue.IsPassed = true;
        }
    }

    public partial class StateIconItem : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private Enum _type;

        [ObservableProperty]
        private bool _isPassed;

        [ObservableProperty]
        private string _iconUnicode = string.Empty;

        public StateIconItem(string title, Enum type, string iconUnicode, bool isPassed = false)
        {
            _title = title;
            _type = type;
            _isPassed = isPassed;
            _iconUnicode = iconUnicode;
        }
    }
}
