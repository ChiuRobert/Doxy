using System.Collections.Generic;
using Entities;

namespace Repositories
{
    internal interface IBaseWordRepository : IRepository
    {
        BaseWord GetById(int id);

        BaseWord GetByWordDialect(string word, Dialect dialect);
        
        List<BaseWord> GetAll();
        
        void Persist(BaseWord entity);

        void Merge(BaseWord entity);

        void Delete(BaseWord entity);
    }
}
