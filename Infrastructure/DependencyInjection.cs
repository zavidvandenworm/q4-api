using System.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<BoardRepository>();
        services.AddScoped<TreeViewRepository>();
        return services;
    }
}