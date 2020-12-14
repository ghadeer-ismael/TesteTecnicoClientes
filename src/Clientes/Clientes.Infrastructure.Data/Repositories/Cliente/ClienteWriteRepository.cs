using Clientes.Domain.Entities;
using Clientes.Domain.Interfaces;
using Clientes.Infrastructure.Data.Context;

namespace Clientes.Infrastructure.Data.Repositories
{
    public class ClienteWriteRepository : BaseWriteRepository<Cliente>, IClienteWriteRepository
    {
        public ClienteWriteRepository(ClientesContext context) : base(context) { }
    }
}
