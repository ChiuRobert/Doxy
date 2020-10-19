using System.Collections.Generic;
using Entities;
using SbLogger;
using SbLogger.Levels;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Utils.Attributes;

namespace UI
{
    public class LanguageActions : MonoBehaviour
    {
        [Inject]
        private static SLogger LOGGER;

        [Inject]
        private LanguageService languageService;
        
        private List<Language> languages;
        private Language selectedLanguage;
        
        public Dropdown languagesDropdown;
        public Text languageName;

        public void Start()
        {
            LOGGER.Log(Level.INFO, "[3]LanguageActions initialized");

            GetAllLanguages();
            PopulateLanguages();

            languagesDropdown.onValueChanged.AddListener(delegate { SelectedLanguage(); });
        }

        public void DeleteLanguage()
        {
            LOGGER.Log(Level.FINE, "Deleting language", new Param {Name = nameof(selectedLanguage), Value = selectedLanguage});
            
            languageService.DeleteLanguage(selectedLanguage);

            Dropdown.OptionData optionData = languagesDropdown.options.Find(x => string.Equals(x.text, selectedLanguage.Name));
            languagesDropdown.options.Remove(optionData);
            
            GetAllLanguages();
        }

        public void SaveLanguage() 
        {
            LOGGER.Log(Level.FINE, "Saving language", new Param {Name = nameof(languageName.text), Value = languageName.text});
            
            string response = languageService.SaveLanguage(languageName.text);

            if (response == null)
            {
                languagesDropdown.options.Add(new Dropdown.OptionData {text = languageName.text});

                GetAllLanguages();
            }
            else
            {
                LOGGER.Log(Level.WARNING, "Language could not be saved", new Param {Name = nameof(response), Value = response});
            }
        }

        public List<Language> GetAllLanguages()
        {
            languages = languageService.GetAllLanguages();
            return languages;
        }

        public Language GetByName(string entityName)
        {
            return languageService.GetByName(entityName);
        }

        private void PopulateLanguages()
        {
            LOGGER.Log(Level.FINE, "Populating language dropdown");
            
            foreach (var language in languages)
            {
                languagesDropdown.options.Add(new Dropdown.OptionData {text = language.Name});
            }
        }

        private void SelectedLanguage()
        {
            selectedLanguage = languages[languagesDropdown.value];
            
            LOGGER.Log(Level.FINE, "Language selected", new Param {Name = nameof(selectedLanguage), Value = selectedLanguage});
        }
    }
}