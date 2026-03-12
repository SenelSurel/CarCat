using System.Collections.ObjectModel;
using CarCat.Models;
using CarCat.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarCat.ViewModels;

public partial class CarsViewModel : BaseViewModel
{
    public int ResultsCount => Cars.Count;
    public bool HasResults => Cars.Count > 0;

    private readonly CarDataService _dataService;

    private List<Car> _allCars = new();

    public ObservableCollection<Car> Cars { get; } = new();

    public ObservableCollection<string> Brands { get; } = new();

    public ObservableCollection<TransmissionType> TransmissionOptions { get; } =
        new() { TransmissionType.Automatic, TransmissionType.Manual };

    [ObservableProperty] private string? yearFromText;
    [ObservableProperty] private string? yearToText;

    [ObservableProperty] private string? engineFromText;
    [ObservableProperty] private string? engineToText;

    partial void OnYearFromTextChanged(string? value) => ApplyFilters();
    partial void OnYearToTextChanged(string? value) => ApplyFilters();
    partial void OnEngineFromTextChanged(string? value) => ApplyFilters();
    partial void OnEngineToTextChanged(string? value) => ApplyFilters();

    partial void OnSearchTextChanged(string? value) => ApplyFilters();

    [ObservableProperty]
    private string? selectedBrand;

    [ObservableProperty]
    private TransmissionType? selectedTransmission;

    [ObservableProperty]
    private string? searchText;

    public CarsViewModel(CarDataService dataService)
    {
        _dataService = dataService;
    }

    [RelayCommand]
    public async Task LoadCarsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var cars = await _dataService.GetCarsAsync();
            _allCars = cars.ToList();

            BuildBrands(_allCars);

            ApplyFilters(); 
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void BuildBrands(List<Car> cars)
    {
        Brands.Clear();
        Brands.Add("All"); 

        foreach (var brand in cars.Select(c => c.Brand).Distinct().OrderBy(b => b))
            Brands.Add(brand);

        if (string.IsNullOrWhiteSpace(SelectedBrand))
            SelectedBrand = "All";
    }

    partial void OnSelectedBrandChanged(string? value) => ApplyFilters();
    partial void OnSelectedTransmissionChanged(TransmissionType? value) => ApplyFilters();

    [RelayCommand]
    public void ClearFilters()
    {
        SelectedBrand = "All";
        SelectedTransmission = null;
        YearFromText = null;
        YearToText = null;
        EngineFromText = null;
        EngineToText = null;
        SearchText = null;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        IEnumerable<Car> query = _allCars;

        
        if (!string.IsNullOrWhiteSpace(SelectedBrand) && SelectedBrand != "All")
            query = query.Where(c => c.Brand == SelectedBrand);

        
        if (SelectedTransmission is not null)
            query = query.Where(c => c.Transmission == SelectedTransmission.Value);

        if(!string.IsNullOrWhiteSpace(SearchText))
        {
            var s = SearchText.Trim();

            query = query.Where(c =>
            (c.Brand?.Contains(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (c.Model?.Contains(s, StringComparison.OrdinalIgnoreCase) ?? false));
        }


        // Parse helpers (empty/invalid => null)
        int? ParseNullableInt(string? text)
            => int.TryParse(text, out var v) ? v : null;

        var yFrom = ParseNullableInt(YearFromText);
        var yTo = ParseNullableInt(YearToText);
        if (yFrom.HasValue && yTo.HasValue && yFrom.Value > yTo.Value)
        {
            // swap
            (yFrom, yTo) = (yTo, yFrom);
        }

        var eFrom = ParseNullableInt(EngineFromText);
        var eTo = ParseNullableInt(EngineToText);
        if (eFrom.HasValue && eTo.HasValue && eFrom.Value > eTo.Value)
        {
            (eFrom, eTo) = (eTo, eFrom);
        }
        
        if (yFrom.HasValue)
            query = query.Where(c => c.Year >= yFrom.Value);

        if (yTo.HasValue)
            query = query.Where(c => c.Year <= yTo.Value);

        
        if (eFrom.HasValue)
            query = query.Where(c => c.EngineDisplacementCc >= eFrom.Value);

        if (eTo.HasValue)
            query = query.Where(c => c.EngineDisplacementCc <= eTo.Value);

        var result = query
            .OrderByDescending(c => c.Year)
            .ThenBy(c => c.Brand)
            .ThenBy(c => c.Model)
            .ToList();

        Cars.Clear();
        foreach (var car in result)
            Cars.Add(car);

        Cars.CollectionChanged += (_,__) => 
        {
            OnPropertyChanged(nameof(ResultsCount));
            OnPropertyChanged(nameof(HasResults));
        };
    }
}