using CarCat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarCat.ViewModels;
using CarCat.Helpers;

namespace CarCat.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _vm;
    public HomePage()
    {
        InitializeComponent();
        _vm = ServiceHelper.Get<HomeViewModel>();
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }

    private async void OnCarsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("cars");
    }
}
