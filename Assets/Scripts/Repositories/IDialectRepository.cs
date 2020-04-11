using Entities;

namespace Repositories
{
    internal interface IDialectRepository : IRepository
    {
        Dialect GetById(int id);
        
        void Persist(Dialect entity);

        void Merge(Dialect entity);

        void Delete(Dialect entity);
    }
}
