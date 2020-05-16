using System.Collections.Generic;
using Controllers;
using Entities;
using SbLogger;
using SbLogger.Levels;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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

        private void Start()
        {
            LOGGER.Log(Level.INFO, "[1]LanguageActions initialized");

            RefreshLanguages();
            PopulateLanguages();

            languagesDropdown.onValueChanged.AddListener(delegate { SelectedLanguage(); });
        }

        public void DeleteLanguage()
        {
            LOGGER.Log(Level.FINE, "Deleting language", new Param {Name = nameof(selectedLanguage), Value = selectedLanguage});
            
            languageService.DeleteLanguage(selectedLanguage);

            Dropdown.OptionData optionData = languagesDropdown.options.Find(x => string.Equals(x.text, selectedLanguage.Name));
            languagesDropdown.options.Remove(optionData);
            
            RefreshLanguages();
        }
        
        public void SaveLanguage() 
        {
            LOGGER.Log(Level.FINE, "Saving language", new Param {Name = nameof(languageName.text), Value = languageName.text});
            
            string response = languageService.SaveLanguage(languageName.text);

            if (response == null)
            {
                languagesDropdown.options.Add(new Dropdown.OptionData() {text = languageName.text});

                RefreshLanguages();
            }
            else
            {
                LOGGER.Log(Level.WARNING, "Language could not be saved", new Param {Name = nameof(response), Value = response});
                print(response);
            }
        }

        private void RefreshLanguages()
        {
            languages = languageService.GetAllLanguages();
        }

        private void PopulateLanguages()
        {
            LOGGER.Log(Level.FINE, "Populating language dropdown");
            
            foreach (var language in languages)
            {
                languagesDropdown.options.Add(new Dropdown.OptionData() {text = language.Name});
            }
        }

        private void SelectedLanguage()
        {
            selectedLanguage = languages[languagesDropdown.value];
            
            LOGGER.Log(Level.FINE, "Language selected", new Param {Name = nameof(selectedLanguage), Value = selectedLanguage});
        }
    }
}