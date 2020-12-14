using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Clientes.Domain.Interfaces
{
    public interface IBaseReadRepository<T> where T : class
    {
        T GetById(Guid id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
    }
}
