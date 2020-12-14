using Clientes.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Clientes.XUnitTest.Context
{
    public class TestClientesContextcs : ClientesContext
    {
        public TestClientesContextcs(DbContextOptions<ClientesContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }
    }
}
