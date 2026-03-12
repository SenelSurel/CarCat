using CarCat.Helpers;
using CarCat.Models;
using CarCat.ViewModels;

namespace CarCat.Views;

public partial class CarsPage : ContentPage
{
    private readonly CarsViewModel _vm;

    public CarsPage()
    {
        InitializeComponent();

        _vm = ServiceHelper.Get<CarsViewModel>();
        BindingContext = _vm;

        // Update EmptyState when list changes
        _vm.Cars.CollectionChanged += (_, __) => UpdateEmptyState();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_vm.Cars.Count == 0)
            await _vm.LoadCarsAsync();

        UpdateEmptyState();
    }

    private void UpdateEmptyState()
    {
        // Loading completed + show if the list is empty
        EmptyState.IsVisible = !_vm.IsBusy && _vm.Cars.Count == 0;
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection?.FirstOrDefault() is not Car selected)
            return;

        ((CollectionView)sender).SelectedItem = null;
        await Shell.Current.GoToAsync($"car-detail?carId={selected.Id}");
    }
}