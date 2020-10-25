using System.Collections.Generic;
using Entities;
using Repositories;
using Repositories.Impl;
using SbLogger;
using SbLogger.Levels;
using Utils;

namespace Services
{
    public class DialectService
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(DialectService), FileService.GetLogPath());

        private readonly IDialectRepository dialectRepository = RepositoryFactory.GetRepository<IDialectRepository>();

        public DialectService()
        {
            LOGGER.Log(Level.INFO, "[2]DialectService initialized");
        }

        public List<Dialect> GetAllDialects()
        {
            return dialectRepository.GetAll();
        }
        
        public Dialect GetByNameLanguage(string name, Language language)
        {
            return dialectRepository.GetByNameLanguage(name, language);
        }
        
        public string SaveDialect(string name, Language language)
        {
            Dialect dialect = new Dialect {Name = name, Language = language};
            Dialect existingDialect = dialectRepository.GetByNameLanguage(name, language);

            if (existingDialect == null)
            {
                dialectRepository.Persist(dialect);
                return null;
            }

            return "The dialect already exists.";
        }
        
        public void DeleteDialect(Dialect dialect)
        {
            dialectRepository.Delete(dialect);
        }
    }
}