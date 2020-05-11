using Entities;
using Repositories;
using Repositories.Impl;
using SbLogger;
using SbLogger.Levels;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Controllers
{
    public class LanguageController : MonoBehaviour
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(LanguageController), FileService.GetLogPath());

        private ILanguageRepository languageRepository;
        
        public new Text name;
        
        private void Awake()
        {
            LOGGER.Log(Level.CONFIG, "LanguageController initialized");
            
            languageRepository = RepositoryFactory.GetRepository<ILanguageRepository>();   
        }

        public void SaveLanguage()
        {
            Language language = new Language {Name = name.text};

            languageRepository.Persist(language);
        }

        public void GetAllLanguages()
        {
            string languages = "";
            var languagesList = languageRepository.GetAll();

            foreach (var language in languagesList)
            {
                languages += language + "\n";
            }

            print(languages);
        }

        public void DeleteLanguage(int id)
        {
            languageRepository.Delete(languageRepository.GetById(id));
        }
    }
}