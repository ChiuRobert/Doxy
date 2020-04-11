using Entities;

namespace Repositories
{
    internal interface IDictionaryRepository : IRepository
    {
        void Persist(Dictionary entity);

        void Merge(Dictionary entity);

        void Delete(Dictionary entity);
    }
}
