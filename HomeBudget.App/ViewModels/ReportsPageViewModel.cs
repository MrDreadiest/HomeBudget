using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.ViewModels.ContentViewModels.Reports;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.Views;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels
{
    public partial class ReportsPageViewModel : BaseViewModel
    {

        public ObservableCollection<ReportCarouselItemBaseViewModel> ReportsCarouselVMs { get; }

        [ObservableProperty]
        private int _carouselPosition;

        private bool _isInternalCarouselPositionChange;

        public SelectableButtonGroupViewModel SelectableButtonGroupVM { get; }

        public ReportsPageViewModel()
        {
            Route = nameof(ReportsPageAndroidView);
            //TODO : Zasoby
            Title = "Raporty";
            IconUnicode = Icons.Graphs;

            ReportsCarouselVMs = new ObservableCollection<ReportCarouselItemBaseViewModel>()
            {
                new ReportGraphViewModel(),
                new ReportTableViewModel()
            };

            SelectableButtonGroupVM = new SelectableButtonGroupViewModel(
                new List<OptionItem> {
                                new OptionItem(ReportType.Graph.GetDescription(),ReportType.Graph),
                                new OptionItem(ReportType.Table.GetDescription(),ReportType.Table),
                });
            SelectableButtonGroupVM.SelectedChanged += SelectableButtonGroupVM_SelectedChanged;
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await Task.Delay(100);

                if (_isInitialized)
                {
                    await ReloadData();
                }
                else
                {
                    _isInitialized = true;
                }

                ResetView();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }

        private void ResetView()
        {
            //throw new NotImplementedException();
        }

        private async Task ReloadData()
        {
            await Task.CompletedTask;
        }

        partial void OnCarouselPositionChanged(int oldValue, int newValue)
        {
            if (!_isInternalCarouselPositionChange)
            {
                SelectableButtonGroupVM.SelectWithoutNotify(newValue);

                ReportsCarouselVMs[newValue].OnAppearingAsync();
                ReportsCarouselVMs[oldValue].OnAppearingAsync();
            }
        }

        private async void SelectableButtonGroupVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is ReportType reportType)
                {
                    int targetPosition = reportType switch
                    {
                        ReportType.Graph => 0,
                        ReportType.Table => 1,
                        _ => CarouselPosition
                    };

                    if (CarouselPosition != targetPosition)
                    {
                        _isInternalCarouselPositionChange = true;

                        await Task.Run(() =>
                        {
                            CarouselPosition = targetPosition;
                        });

                        await Task.Delay(100);
                        _isInternalCarouselPositionChange = false;
                    }
                }
            }
        }


    }
}
