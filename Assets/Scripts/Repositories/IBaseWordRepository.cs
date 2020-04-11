using Entities;

namespace Repositories
{
    internal interface IBaseWordRepository : IRepository
    {
        void Persist(BaseWord entity);

        void Merge(BaseWord entity);

        void Delete(BaseWord entity);
    }
}
