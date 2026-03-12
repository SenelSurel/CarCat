using Microsoft.Extensions.Logging;
using CarCat.Services;
using CarCat.ViewModels;
using CarCat.Views;
using CommunityToolkit.Maui;

namespace CarCat;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();

        builder.Services.AddSingleton<CarDataService>();

     
        builder.Services.AddTransient<CarsViewModel>();
        builder.Services.AddTransient<CarDetailViewModel>();

        
        builder.Services.AddTransient<CarsPage>();
        builder.Services.AddTransient<CarDetailPage>();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HomePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}