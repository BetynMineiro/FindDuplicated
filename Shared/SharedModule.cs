using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using Shared.Services.Interfaces;

namespace Shared;

public static class SharedModule
{
    public static void ConfigureSharedLayerServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileService, FileService>();
  
    }
    public static void ConfigureMessageHandler(this IServiceCollection services, List<Assembly> assemblyCollection)
    {
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssemblies(assemblyCollection.ToArray());
        });
    }
}