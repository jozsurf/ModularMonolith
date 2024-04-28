using AuthorisationModule.BL;
using AuthorisationModule.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorisationModule;

public static class DependencyInjectionHelper
{
    public static void RegisterAuthorisationModuleDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorisationRepository, InMemoryAuthorisationRepository>();
        services.AddTransient<IAuthorisationManager, AuthorisationManager>();
    }
}