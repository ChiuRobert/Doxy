using System.Collections.Generic;
using Entities;

namespace Repositories
{
    internal interface IDictionaryRepository : IRepository
    {
        Dictionary GetById(int id);

        Dictionary GetByBaseTranslated(BaseWord baseWord, BaseWord translatedWord);
        
        List<Dictionary> GetAll();
        
        void Persist(Dictionary entity);

        void Merge(Dictionary entity);

        void Delete(Dictionary entity);
    }
}
