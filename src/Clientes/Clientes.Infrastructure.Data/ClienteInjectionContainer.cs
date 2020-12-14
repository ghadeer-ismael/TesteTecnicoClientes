using Clientes.Domain.Interfaces;
using Clientes.Infrastructure.Data.Context;
using Clientes.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clientes.Infrastructure.Data
{
    public static class ClienteInjectionContainer
    {
        public static IServiceCollection ConfigureDependecies(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<ClientesContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IClienteReadRepository, ClienteReadRepository>();
            services.AddScoped<IClienteWriteRepository, ClienteWriteRepository>();

            return services;
        }
    }
}