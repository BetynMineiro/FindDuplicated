using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DomainModule
{
    public static void ConfigureDomainLayerServices(this IServiceCollection services)
    {
        services.AddSingleton<IMatchContactService, MatchContactService>();
    }
}