using System.Collections.Generic;
using Entities;
using Repositories;
using Repositories.Impl;
using SbLogger;
using SbLogger.Levels;
using Utils;

namespace Services
{
    public class DictionaryService
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(DictionaryService), FileService.GetLogPath());

        private readonly IDictionaryRepository dictionaryRepository = RepositoryFactory.GetRepository<IDictionaryRepository>();

        public DictionaryService()
        {
            LOGGER.Log(Level.INFO, "[2]DictionaryService initialized");
        }

        public List<Dictionary> GetAllDictionaries()
        {
            return dictionaryRepository.GetAll();
        }
        
        public Dictionary GetByBaseTranslated(BaseWord baseWord, BaseWord translatedWord)
        {
            return dictionaryRepository.GetByBaseTranslated(baseWord, translatedWord);
        }
        
        public string SaveDictionary(BaseWord baseWord, BaseWord translatedWord)
        {
            Dictionary dictionary = new Dictionary {BaseWord = baseWord, TranslatedWord = translatedWord};
            Dictionary translatedDictionary = new Dictionary {BaseWord = translatedWord, TranslatedWord = baseWord};
            
            Dictionary existingDictionary = dictionaryRepository.GetByBaseTranslated(baseWord, translatedWord);

            if (existingDictionary == null)
            {
                dictionaryRepository.Persist(dictionary);
                dictionaryRepository.Persist(translatedDictionary);
                return null;
            }

            return "The dictionary already exists.";
        }
        
        public void DeleteDictionary(Dictionary dictionary)
        {
            dictionaryRepository.Delete(dictionary);
        }
    }
}