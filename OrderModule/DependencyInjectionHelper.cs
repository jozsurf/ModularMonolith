using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using OrderModule.BL;
using OrderModule.Data;

// for unit tests
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("OrderModuleTests")]
namespace OrderModule;

public static class DependencyInjectionHelper
{
    public static void RegisterOrderModuleDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddTransient<IOrderManager, OrderManager>();
    }
}