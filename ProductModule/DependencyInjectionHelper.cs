using Microsoft.Extensions.DependencyInjection;
using ProductModule.BL;
using ProductModule.Data;

namespace ProductModule;

public static class DependencyInjectionHelper
{
    public static void RegisterProductModuleDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        services.AddTransient<IProductManager, ProductManager>();
    }
}