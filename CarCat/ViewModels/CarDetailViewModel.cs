using CarCat.Models;
using CarCat.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CarCat.ViewModels;

public partial class CarDetailViewModel : BaseViewModel
{
    private readonly CarDataService _dataService;

    [ObservableProperty]
    private Car? car;

    [ObservableProperty]
    private ObservableCollection<string> sliderImages = new();

    public bool HasCar => Car is not null;
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public CarDetailViewModel(CarDataService dataService)
    {
        _dataService = dataService;
    }

    public async Task LoadAsync(int carId)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            ErrorMessage = null;
            Car = null;
            OnPropertyChanged(nameof(HasCar));
            OnPropertyChanged(nameof(HasError));

            var cars = await _dataService.GetCarsAsync();
            Car = cars.FirstOrDefault(c => c.Id == carId);

            OnPropertyChanged(nameof(HasCar));

            if (Car is null)
            {
                ErrorMessage = $"Car not found (Id: {carId}).";
                OnPropertyChanged(nameof(HasError));
            }

            if (Car.Images is not null && Car.Images.Count > 0)
            {
                foreach (var image in Car.Images)
                    SliderImages.Add(image);
            }

        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            OnPropertyChanged(nameof(HasError));
        }
        finally
        {
            IsBusy = false;
        }
    }
}