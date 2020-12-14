using Clientes.Domain.Entities;
using Clientes.Domain.Interfaces;
using Clientes.Infrastructure.Data.Context;

namespace Clientes.Infrastructure.Data.Repositories
{
    public class ClienteReadRepository : BaseReadRepository<Cliente>, IClienteReadRepository
    {
        public ClienteReadRepository(ClientesContext context) : base(context) { }
    }
}
