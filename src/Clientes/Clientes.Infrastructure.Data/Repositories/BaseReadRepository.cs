using Clientes.Domain.Interfaces;
using Clientes.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Clientes.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Aqui são os metódos de listar, procurar os registros
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseReadRepository<T> : IDisposable, IBaseReadRepository<T> where T : class
    {
        private readonly ClientesContext _context;

        public BaseReadRepository(ClientesContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /// <summary>
        ///     Procurar por predicado
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>
        ///     <see cref="IEnumerable{T}" />
        /// </returns>
        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = _context.Set<T>().AsNoTracking().Where(predicate).ToArray();
            return query;
        }

        /// <summary>
        ///     Listar todos <see cref="IEnumerable{T}" />
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToArray();
        }

        /// <summary>
        ///     Retornar um registro
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}