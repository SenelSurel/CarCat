using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CarCat.Helpers
{
    public static class ServiceHelper
    {
        public static T Get<T>() where T : notnull
        {
            var services=Application.Current?.Handler?.MauiContext?.Services
            
               ?? throw new InvalidOperationException("DI container is not available yet.");
                return services.GetRequiredService<T>();
            
        }

    }
}
