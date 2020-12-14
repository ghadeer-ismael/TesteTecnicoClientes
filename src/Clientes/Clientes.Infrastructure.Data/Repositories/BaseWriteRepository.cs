using Clientes.Domain.Interfaces;
using Clientes.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Clientes.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Aqui são os metódos de adicionar, alterar e excluir registros
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseWriteRepository<T> : IDisposable, IBaseWriteRepository<T> where T : class
    {
        private readonly ClientesContext _context;

        public BaseWriteRepository(ClientesContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Adicionar um novo registro
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        ///     Excluir um registro
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        /// <summary>
        ///     Atualizar um registro
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}