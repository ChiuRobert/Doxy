using System.Collections.Generic;
using Entities;

namespace Repositories
{
    internal interface ILanguageRepository : IRepository
    {
        Language GetById(int id);

        List<Language> GetAll();
        
        void Persist(Language entity);

        void Merge(Language entity);

        void Delete(Language entity);
    }
}
