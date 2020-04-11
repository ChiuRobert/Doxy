using Entities;
using SbLogger;
using Utils;

namespace Repositories.Impl
{
    internal class BaseWordRepository : IBaseWordRepository
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(BaseWordRepository), FileService.GetLogPath());

        public void Delete(BaseWord entity)
        {
            throw new System.NotImplementedException();
        }

        public void Merge(BaseWord entity)
        {
            throw new System.NotImplementedException();
        }

        public void Persist(BaseWord entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
