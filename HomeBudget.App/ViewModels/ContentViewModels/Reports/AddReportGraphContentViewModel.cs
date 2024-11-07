using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.Reports
{
    public partial class AddReportGraphContentViewModel : AddReportContentViewModelBase
    {
        [RelayCommand]
        public void Next()
        {
            if (CarouselPosition < Images.Count - 1)
            {
                CarouselPosition++;
            }
        }

        [RelayCommand]
        public void Back()
        {
            if (CarouselPosition > 0)
            {
                CarouselPosition--;
            }
        }

        public ObservableCollection<CarouselGraphImage> Images { get; }

        [ObservableProperty]
        private CarouselGraphImage _selectedImage;

        [ObservableProperty]
        private int _carouselPosition;

        public AddReportGraphContentViewModel() : base(ReportType.Graph)
        {
            IsCollapsed = true;

            Images = new ObservableCollection<CarouselGraphImage>(Enum.GetValues(typeof(ReportGraphType)).Cast<ReportGraphType>().Select(g => new CarouselGraphImage(g)));
        }
    }

    public class CarouselGraphImage
    {
        public ReportGraphType ReportGraphType { get; }
        public string ImageSource => ReportGraphType.GetImageSource();
        public string Description => ReportGraphType.GetDescription();

        public CarouselGraphImage(ReportGraphType reportGraphType)
        {
            ReportGraphType = reportGraphType;
        }
    }
}