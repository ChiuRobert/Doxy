using Doxy.Entities;

namespace Doxy.Repositories
{
    interface IRepository<T> where T : IModel
    {
        void Persist(T entity);

        T Merge(T entity);

        void Delete(T entity);
    }
}
