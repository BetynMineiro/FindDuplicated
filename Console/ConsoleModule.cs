using System.Reflection;
using Application;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Console;

public static class ConsoleModule
{
    public static void ConfigureMessageHandlerLayer(this IServiceCollection services)
    {
        var assemblyCollection = new List<Assembly>
        {
            typeof(ApplicationModule).Assembly,
        };
        services.ConfigureMessageHandler(assemblyCollection);
    }
}