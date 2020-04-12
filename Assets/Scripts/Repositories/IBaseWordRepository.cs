using Entities;

namespace Repositories
{
    internal interface IBaseWordRepository : IRepository
    {
        BaseWord GetById(int id);
        
        void Persist(BaseWord entity);

        void Merge(BaseWord entity);

        void Delete(BaseWord entity);
    }
}
