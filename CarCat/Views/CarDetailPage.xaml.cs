using CarCat.Helpers;
using CarCat.ViewModels;

namespace CarCat.Views;

[QueryProperty(nameof(CarId), "carId")]
public partial class CarDetailPage : ContentPage
{
    private readonly CarDetailViewModel _vm;

    private string? _carId;
    public string? CarId
    {
        get => _carId;
        set
        {
            _carId = value;
            _ = LoadCarAsync(value);
        }
    }
    private int _currentIndex = 0;


    public CarDetailPage(CarDetailViewModel vm)
    {
        InitializeComponent();
        _vm = ServiceHelper.Get<CarDetailViewModel>();
        BindingContext = _vm;

        Task.Delay(2000);
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1.5), () =>
        {
            if (carCarousel.ItemsSource is null)
                return true;

            var count = _vm.SliderImages.Count;
            if (count == 0)
                return true;

            _currentIndex = (_currentIndex + 1) % count;
            carCarousel.Position = _currentIndex;

            return true;
        });
    }

    private async Task LoadCarAsync(string? carIdText)
    {
        if (!int.TryParse(carIdText, out var carId))
        {
            await _vm.LoadAsync(-1);
            return;
        }

        await _vm.LoadAsync(carId);
    }
}