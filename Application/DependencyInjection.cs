using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using ShipmentManagement.Application.Interfaces;
using ShipmentManagement.Application.Services;


namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IShipmentService, ShipmentService>();

        return services;
    }
}