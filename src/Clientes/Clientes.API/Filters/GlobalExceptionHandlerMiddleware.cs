using Clientes.Domain.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Clientes.API.Filters
{
    /// <summary>
    /// Middleware Customizado para pegar os erros
    /// </summary>
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Middleware Customizado para pegar os erros
        /// </summary>
        /// <param name="logger"></param>
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// O método InvokeAsync será chamado automaticamente, nele existe uma chamada ao método next(context) que irá executar o próximo passo do pipeline 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                //quando houver um erro, retorne uma mensagem de erro json ao cliente
                if (error != null && error.Error != null)
                    throw new Exception("Erro do servidor interno", new Exception(error.Error.Message));

                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var json = new BaseResponse(exception);
                string resultado = JsonConvert.SerializeObject(json);

                return context.Response.WriteAsync(resultado);
            }
            catch
            {
                var json = new BaseResponse
                {
                    Success = false,
                    Message = "Ocorreu um erro",
                    Errors = new string[] { "Falha ao analizar o erro, tente novamente." }
                };

                return context.Response.WriteAsync(JsonConvert.SerializeObject(json));
            }
        }
    }
}
