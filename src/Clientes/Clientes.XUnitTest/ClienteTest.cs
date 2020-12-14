using AutoMapper;
using Clientes.API.Controllers;
using Clientes.Domain.Entities;
using Clientes.Domain.Interfaces;
using Clientes.Domain.MappingProfiles;
using Clientes.Infrastructure.Data.Context;
using Clientes.Infrastructure.Data.Repositories;
using Clientes.XUnitTest.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Clientes.XUnitTest
{
    public class ClienteTest : IDisposable
    {
        private TestClientesContextcs testClientesContextcs { get; set; }
        private IClienteReadRepository clienteReadRepository { get; set; }
        private IClienteWriteRepository clienteWriteRepository { get; set; }
        private IMapper mapper { get; set; }

        public ClientesController clientesController { get; private set; }

        public ClienteTest()
        {
            DbContextOptions<ClientesContext> contextOptions = new DbContextOptionsBuilder<ClientesContext>().Options;
            testClientesContextcs = new TestClientesContextcs(contextOptions);

            testClientesContextcs.Clientes.AddRange(new List<Cliente>()
                {
                    new Cliente("Exemplo cliente 001", 15) { Id = new Guid("078C89D3-7D66-4DE3-A117-A80D69391675") },
                    new Cliente("Exemplo cliente 002", 25){ Id = new Guid("EC01A195-4DAB-4F25-BD35-54FD2B418D43") },
                    new Cliente("Exemplo cliente 003", 30)
                });

            testClientesContextcs.SaveChanges();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mappingConfig.CreateMapper();

            clienteReadRepository = new ClienteReadRepository(testClientesContextcs);
            clienteWriteRepository = new ClienteWriteRepository(testClientesContextcs);
            clientesController = new ClientesController(clienteReadRepository, clienteWriteRepository, mapper);
        }

        ~ClienteTest()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // limpar a memória
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                testClientesContextcs.Dispose();
                testClientesContextcs = null;

                clienteReadRepository = null;
                clienteWriteRepository = null;
                mapper = null;

                clientesController = null;
            }
        }
    }
}
