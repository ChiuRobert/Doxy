using Entities;

namespace Repositories
{
    internal interface ILanguageRepository : IRepository
    {
        Language GetById(int id);
        
        void Persist(Language entity);

        void Merge(Language entity);

        void Delete(Language entity);
    }
}
