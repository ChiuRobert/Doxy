using System.Collections.Generic;
using Entities;
using Repositories;
using Repositories.Impl;
using SbLogger;
using SbLogger.Levels;
using Utils;

namespace Services
{
    public class BaseWordService
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(BaseWordService), FileService.GetLogPath());

        private readonly IBaseWordRepository baseWordRepository = RepositoryFactory.GetRepository<IBaseWordRepository>();

        public BaseWordService()
        {
            LOGGER.Log(Level.INFO, "[2]BaseWordService initialized");
        }
        
        public List<BaseWord> GetAllBaseWords()
        {
            return baseWordRepository.GetAll();
        }
        
        public BaseWord GetByWordDialect(string word, Dialect dialect)
        {
            return baseWordRepository.GetByWordDialect(word, dialect);
        }
        
        public string SaveBaseWord(string word, Dialect dialect)
        {
            BaseWord baseWord = new BaseWord {Word = word, Dialect = dialect};
            BaseWord existingBaseWord = baseWordRepository.GetByWordDialect(word, dialect);

            if (existingBaseWord == null)
            {
                baseWordRepository.Persist(baseWord);
                return null;
            }

            return "The BaseWord already exists.";
        }
        
        public void DeleteBaseWord(BaseWord baseWord)
        {
            baseWordRepository.Delete(baseWord);
        }
    }
}