using System.Collections.Generic;
using Entities;
using SbLogger;
using SbLogger.Levels;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Utils.Attributes;

namespace UI.Impl
{
    [RequireComponent(typeof(IBaseWordActions))]
    public class DictionaryActions : MonoBehaviour
    {
        [Inject]
        private static SLogger LOGGER;

        [Inject]
        private DictionaryService dictionaryService;

        private IBaseWordActions baseWordActions;
        private IBaseWordActions translatedWordActions;
        
        private List<Dictionary> dictionaries;
        private Dictionary selectedDictionary;
        
        public Dropdown dictionariesDropdown;
        
        public void Start()
        {
            LOGGER.Log(Level.INFO, "[3]DictionaryActions initialized");

            IBaseWordActions[] iBaseWordActions = GetComponents<IBaseWordActions>();

            baseWordActions = iBaseWordActions[0];
            translatedWordActions = iBaseWordActions[1];
            
            GetAllDictionaries();
            PopulateDictionaries();

            dictionariesDropdown.onValueChanged.AddListener(delegate { SelectedDictionary(); });
        }

        public void SaveDictionary()
        {
            var baseWord = baseWordActions.GetSelectedBaseWord();
            var translatedWord = translatedWordActions.GetSelectedBaseWord();
            
            LOGGER.Log(Level.FINE, "Saving dictionary", 
                new Param[]
                {
                    new Param {Name = nameof(baseWord), Value = baseWord},
                    new Param {Name = nameof(translatedWord), Value = translatedWord}
                });

            if (baseWord == null || translatedWord == null)
            {
                LOGGER.Log(Level.SEVERE, "Dictionary could not be saved because the selected BaseWord/TranslatedWord is null");
                return;
            }
            
            string response = dictionaryService.SaveDictionary(baseWord, translatedWord);

            if (response == null)
            {
                dictionariesDropdown.options.Add(new Dropdown.OptionData {text = baseWord.Word + " = " + translatedWord.Word});

                GetAllDictionaries();
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "Dictionary could not be saved", new Param {Name = nameof(response), Value = response});
            }
        }

        public void DeleteDictionary()
        {
            LOGGER.Log(Level.FINE, "Deleting dictionary", new Param {Name = nameof(selectedDictionary), Value = selectedDictionary});
            
            dictionaryService.DeleteDictionary(selectedDictionary);

            Dictionary alternateDictionary =
                GetByBaseTranslated(selectedDictionary.TranslatedWord, selectedDictionary.BaseWord);
            
            LOGGER.Log(Level.FINE, "Deleting alternate dictionary", new Param {Name = nameof(alternateDictionary), Value = alternateDictionary});
            
            Dropdown.OptionData optionData = dictionariesDropdown.options.Find(x =>
                string.Equals(x.text,
                    selectedDictionary.BaseWord.Word + " = " + selectedDictionary.TranslatedWord.Word));
            dictionariesDropdown.options.Remove(optionData);
            
            GetAllDictionaries();
        }
        
        public List<Dictionary> GetAllDictionaries()
        {
            dictionaries = dictionaryService.GetAllDictionaries();
            return dictionaries;
        }
        
        public Dictionary GetByBaseTranslated(BaseWord baseWord, BaseWord translatedWord)
        {
            return dictionaryService.GetByBaseTranslated(baseWord, translatedWord);
        }
        
        private void PopulateDictionaries()
        {
            LOGGER.Log(Level.FINE, "Populating dictionary dropdown");
            
            foreach (var dictionary in dictionaries)
            {
                dictionariesDropdown.options.Add(new Dropdown.OptionData
                    {text = dictionary.BaseWord.Word + " = " + dictionary.TranslatedWord.Word});
            }
        }

        private void SelectedDictionary()
        {
            selectedDictionary = dictionaries[dictionariesDropdown.value];
            
            LOGGER.Log(Level.FINE, "Dictionary selected", new Param {Name = nameof(selectedDictionary), Value = selectedDictionary});
        }
    }
}