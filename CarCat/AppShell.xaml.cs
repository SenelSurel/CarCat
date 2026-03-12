using CarCat.Views;

namespace CarCat;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("cars", typeof(CarsPage));
        Routing.RegisterRoute("car-detail", typeof(CarDetailPage));
    }
}