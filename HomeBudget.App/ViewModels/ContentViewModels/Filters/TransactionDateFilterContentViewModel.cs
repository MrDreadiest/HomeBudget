using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;

namespace HomeBudget.App.ViewModels.ContentViewModels.Filters
{
    public partial class TransactionDateFilterContentViewModel : FilterContentViewModelBase, ITransactionFilter
    {
        [ObservableProperty]
        private SelectableButtonGroupViewModel selectableButtonGroupVM;

        [ObservableProperty]
        private DateTime _dateFrom;

        [ObservableProperty]
        private DateTime _dateTo;

        public TransactionDateFilterContentViewModel()
        {
            // TODO: Przeniesienie do zasobów
            Title = "Data";
            SelectableButtonGroupVM = new SelectableButtonGroupViewModel(
                new List<OptionItem> {
                    new OptionItem("Własny",DateType.Own),
                    new OptionItem("Dzisiaj",DateType.Today),
                    new OptionItem("Ten miesiąc",DateType.ThisMonth),
                    new OptionItem("Poprzeni miesiąc",DateType.LastMonth),
                });
            SelectableButtonGroupVM.SelectedChanged += SelectableButtonGroupVM_SelectedChanged;

            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
        }

        public IEnumerable<TransactionGroup> Apply(IEnumerable<TransactionGroup> transactions)
        {
            return transactions.Where(group =>
                group.Date >= DateFrom && group.Date <= DateTo);
        }

        public async void Reply()
        {
            await Refilter();
        }

        private void SelectableButtonGroupVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is DateType dateType)
                {
                    var today = DateTime.Today;

                    switch (dateType)
                    {
                        case DateType.Own:
                            DateFrom = today;
                            DateTo = today;
                            break;

                        case DateType.Today:
                            DateFrom = today;
                            DateTo = today;
                            break;

                        case DateType.ThisMonth:
                            DateFrom = new DateTime(today.Year, today.Month, 1);
                            DateTo = DateFrom.AddMonths(1).AddDays(-1);
                            break;

                        case DateType.LastMonth:
                            DateFrom = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                            DateTo = new DateTime(today.Year, today.Month, 1).AddDays(-1);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public enum DateType
        {
            Own,
            Today,
            ThisMonth,
            LastMonth
        }
    }
}
