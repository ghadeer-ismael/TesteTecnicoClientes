using Clientes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Clientes.Infrastructure.Data.Context
{
    public class ClientesContext : DbContext
    {
        public ClientesContext(DbContextOptions<ClientesContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }

        //Aplicar as configurações das entidades antes de serem criadas no banco de dados.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Para não excluir os registros relacionados com o registro excluído.
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.NoAction;

            //builder.ApplyConfiguration(new ClienteConfig());
            base.OnModelCreating(builder);

            /* Para não adicionar as configurações para cada entidade, 
             * esta linha irá procurar as configurações como na pasta "EntityConfig" e aplicá-las. */
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        // Aqui pode alterar as informações antes de salvál-as no banco de dados
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Listar todas as entidades que tem itens novos ou modificados.
            var changedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

            // Atualizar as colunas "DataCricao" e "DataAlteracao" se existem.
            foreach (var e in changedEntities)
            {
                var dateTime = DateTime.Now;

                if (e.State == EntityState.Added)
                {
                    var created = e.Entity.GetType().GetProperty("DataCriacao");
                    if (created != null) created.SetValue(e.Entity, dateTime);
                }

                if (e.State == EntityState.Modified)
                {
                    var updated = e.Entity.GetType().GetProperty("DataAlteracao");
                    if (updated != null) updated.SetValue(e.Entity, dateTime);
                }
            };

            return base.SaveChangesAsync(cancellationToken);
        }

        // Aqui pode alterar as informações antes de salvál-as no banco de dados
        public override int SaveChanges()
        {
            // Listar todas as entidades que tem itens novos ou modificados.
            var changedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

            // Atualizar as colunas "DataCricao" e "DataAlteracao" se existem.
            foreach (var e in changedEntities)
            {
                var dateTime = DateTime.Now;

                if (e.State == EntityState.Added)
                {
                    var created = e.Entity.GetType().GetProperty("DataCriacao");
                    if (created != null) created.SetValue(e.Entity, dateTime);
                }

                if (e.State == EntityState.Modified)
                {
                    var updated = e.Entity.GetType().GetProperty("DataAlteracao");
                    if (updated != null) updated.SetValue(e.Entity, dateTime);
                }
            };

            return base.SaveChanges();
        }
    }
}
