using Entities;

namespace Repositories
{
    internal interface IDictionaryRepository : IRepository
    {
        Dictionary GetById(int id);
        
        void Persist(Dictionary entity);

        void Merge(Dictionary entity);

        void Delete(Dictionary entity);
    }
}
