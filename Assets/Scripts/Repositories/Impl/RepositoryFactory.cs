using SbLogger;
using SbLogger.Levels;
using Utils;

namespace Repositories.Impl
{
    internal static class RepositoryFactory
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(RepositoryFactory), FileService.GetLogPath());

        public static T GetRepository<T>() where T : IRepository
        {
            LOGGER.Log(Level.INFO, "[1]Creating repository", new Param {Name = nameof(T), Value = typeof(T)});
            
            T result = default(T);

            if (typeof(T) == typeof(IBaseWordRepository))
            {
                return (T)(IBaseWordRepository)new BaseWordRepository();
            }

            if (typeof(T) == typeof(ILanguageRepository))
            {
                return (T)(ILanguageRepository)new LanguageRepository();
            }
            
            if (typeof(T) == typeof(IDialectRepository))
            {
                return (T)(IDialectRepository)new DialectRepository();
            }
            
            if (typeof(T) == typeof(IDictionaryRepository))
            {
                return (T)(IDictionaryRepository)new DictionaryRepository();
            }
            
            return result;
        }
    }
}