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

        private readonly ILanguageRepository languageRepository;
        
        public LanguageService()
        {
            LOGGER.Log(Level.INFO, "[1]LanguageService initialized");
            
            languageRepository = RepositoryFactory.GetRepository<ILanguageRepository>();
        }
        
        public List<Language> GetAllLanguages()
        {
            return languageRepository.GetAll();
        }

        public string SaveLanguage(string name)
        {
            Language language = new Language {Name = name};

            Language existingLanguage = languageRepository.GetByName(name);

            if (existingLanguage == null)
            {
                languageRepository.Persist(language);
                return null;
            }

            return "The language already exists.";
        }

        public void DeleteLanguage(Language language)
        {
            languageRepository.Delete(languageRepository.GetById(language.Id));
        }
    }
}