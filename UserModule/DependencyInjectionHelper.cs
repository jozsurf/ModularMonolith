using Microsoft.Extensions.DependencyInjection;
using UserModule.BL;
using UserModule.Data;

namespace UserModule;

public static class DependencyInjectionHelper
{
    public static void RegisterUserModuleDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddTransient<IUserManager, UserManager>();
    }
}