using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Clientes.API.Filters
{
    /// <summary>
    /// Extensão para tratar os erros
    /// </summary>
    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Adicionar Global Exception Handler Middleware
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGlobalExceptionHandlerMiddleware(this IServiceCollection services)
        {
            return services.AddTransient<GlobalExceptionHandlerMiddleware>();
        }

        /// <summary>
        /// Usar o Global Exception Handler Middleware
        /// </summary>
        /// <param name="app"></param>
        public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}