using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Clientes.API.Filters;
using Clientes.Domain.MappingProfiles;
using Clientes.Domain.Shared;
using Clientes.Infrastructure.Data;
using Clientes.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Clientes.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers();

            // Registrar as dependências de infra-estrutura
            services.ConfigureDependecies(Configuration);

            services.AddSingleton<MappingProfile>();
            services.AddAutoMapper(o => o.AddProfile(new MappingProfile()));

            //Middleware Customizado
            services.AddGlobalExceptionHandlerMiddleware();

            //Tratar os erros de validação
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState);

                    var result = new BadRequestObjectResult(problemDetails);
                    try
                    {
                        BaseResponse respostaApi = new BaseResponse()
                        {
                            Success = false,
                            Message = problemDetails.Title == "One or more validation errors occurred." ? "Ocorreram um ou mais erros de validação." : problemDetails.Title,
                            Errors = problemDetails?.Errors?.Select(x => string.Join("; ", x.Value.ToArray()).ToLower().Contains("campo") ? string.Join("; ", x.Value.ToArray()) : x.Key + ": " + string.Join("; ", x.Value.ToArray())).ToArray()
                        };
                        result = new BadRequestObjectResult(respostaApi);
                    }
                    catch
                    {
                        result = new BadRequestObjectResult(problemDetails);
                    }

                    result.ContentTypes.Add("application/problem+json");
                    result.ContentTypes.Add("application/problem+xml");

                    return result;
                };
            });

            // Gerar a tela de swagger
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API - Teste Cliente",
                    Description = "API Swagger - Teste Cliente",
                    Contact = new OpenApiContact
                    {
                        Name = "Ghadeer Ismael",
                        Email = "ghadeersy@gmail.com"
                    }
                });
                s.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Criar o banco de dados e inserir os dados iniciais automaticamente.
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var Db = scope.ServiceProvider.GetRequiredService<ClientesContext>();
                ClientesContextSeed.Seed(Db, loggerFactory, env.IsDevelopment());
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Criar o Swagger da API
            app.UseSwagger();

            // Configurar a tela do Swagger da API
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "API - Teste Cliente v1");
                c.RoutePrefix = "docs";
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());

            app.UseAuthorization();

            //Usar Middleware Customizado
            app.UseGlobalExceptionHandlerMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
