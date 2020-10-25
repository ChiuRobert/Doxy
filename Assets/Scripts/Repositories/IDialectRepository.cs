using System.Collections.Generic;
using Entities;

namespace Repositories
{
    internal interface IDialectRepository : IRepository
    {
        Dialect GetById(int id);

        Dialect GetByNameLanguage(string name, Language language);
        
        List<Dialect> GetAll();
        
        void Persist(Dialect entity);

        void Merge(Dialect entity);

        void Delete(Dialect entity);
    }
}
