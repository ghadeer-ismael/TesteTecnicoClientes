namespace Clientes.Domain.Interfaces
{
    public interface IBaseWriteRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
