using Microsoft.Extensions.DependencyInjection;
using OrderModule.BL;
using OrderModule.Data;

namespace OrderModule;

public static class DependencyInjectionHelper
{
    public static void RegisterOrderModuleDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddTransient<IOrderManager, OrderManager>();
    }
}