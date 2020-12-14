using Clientes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clientes.Infrastructure.Data.Context
{
    public class ClientesContextSeed
    {
        public static void Seed(ClientesContext clientesContext, ILoggerFactory loggerFactory, bool IsDevelopment = false)
        {
            try
            {
                //Criar o banco de dados e aplicar qualquer alteração pendente.
                clientesContext.Database.Migrate();

                //Inserir alguns dados se a tabela está vazia.
                if (!clientesContext.Clientes.Any())
                {
                    clientesContext.Clientes.AddRange(GetPreconfiguredClientes());
                    clientesContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var log = loggerFactory.CreateLogger<ClientesContextSeed>();
                log.LogError(ex.Message);

                throw ex;
            }

            // Alguns dados a serem inseridos na tabela se estiver vazia
            static IEnumerable<Cliente> GetPreconfiguredClientes()
            {
                return new List<Cliente>()
                {
                    new Cliente("Exemplo cliente 1", 45),
                    new Cliente("Exemplo cliente 2", 35),
                    new Cliente("Exemplo cliente 3", 40)
                };
            }
        }
    }
}
