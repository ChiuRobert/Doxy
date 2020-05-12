using System.Collections.Generic;
using Entities;
using Repositories;
using Repositories.Impl;
using SbLogger;
using SbLogger.Levels;
using Utils;

namespace Controllers
{
    public class LanguageService
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(LanguageService), FileService.GetLogPath());

        private readonly ILanguageRepository languageRepository = RepositoryFactory.GetRepository<ILanguageRepository>();
        
        public LanguageService()
        {
            LOGGER.Log(Level.INFO, "[3]LanguageService initialized");
        }
        
        public List<Language> GetAllLanguages()
        {
            return languageRepository.GetAll();
        }

        public void SaveLanguage(string name)
        {
            Language language = new Language {Name = name};

            languageRepository.Persist(language);
        }

        public void DeleteLanguage(Language language)
        {
            languageRepository.Delete(languageRepository.GetById(language.Id));
        }
    }
}