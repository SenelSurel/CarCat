using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarCat.Services;
using System.Linq.Expressions;

namespace CarCat.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly CarDataService _dataService;

        [ObservableProperty] 
        private string carsCountText = "Total cars: -";

        public HomeViewModel()
        {
            _dataService = new CarDataService();
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if(IsBusy)
                return;

            try
            {
                IsBusy = true;
                var cars = await _dataService.GetCarsAsync();
                CarsCountText = $"We currently have {cars.Count} cars";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
