using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using CarCat.Models;

namespace CarCat.Services;

public sealed class CarDataService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IReadOnlyList<Car>> GetCarsAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("Data/cars.json");
        using var reader = new StreamReader(stream);

        var json = await reader.ReadToEndAsync();
        var cars = JsonSerializer.Deserialize<List<Car>>(json,_jsonOptions) ?? new List<Car>();

   
        return cars
            .GroupBy(c => c.Id)
            .Select(g => g.First())
            .OrderByDescending(c => c.Year)
            .ToList();
    }
}
