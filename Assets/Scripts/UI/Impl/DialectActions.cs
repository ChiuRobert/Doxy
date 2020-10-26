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
    [RequireComponent(typeof(ILanguageActions))]
    public class DialectActions : MonoBehaviour, IDialectActions
    {
        [Inject]
        private static SLogger LOGGER;

        [Inject]
        private DialectService dialectService;

        private ILanguageActions languageActions;
        
        private List<Dialect> dialects;
        private Dialect selectedDialect;
        
        public Dropdown dialectsDropdown;
        public Text dialectName;
        
        public void Start()
        {
            LOGGER.Log(Level.INFO, "[3]DialectActions initialized");

            languageActions = GameObject.Find("Actions").GetComponent<ILanguageActions>();
            
            GetAllDialects();
            PopulateDialects();

            dialectsDropdown.onValueChanged.AddListener(delegate { SelectedDialect(); });
        }

        public void SaveDialect()
        {
            var selectedLanguage = languageActions.GetSelectedLanguage();
            
            LOGGER.Log(Level.FINE, "Saving dialect", 
                new Param[]
                {
                    new Param {Name = nameof(dialectName.text), Value = dialectName.text},
                    new Param {Name = nameof(selectedLanguage), Value = selectedLanguage}
                });

            if (selectedLanguage == null)
            {
                LOGGER.Log(Level.SEVERE, "Dialect could not be saved because the selected language is null");
                return;
            }
            
            string response = dialectService.SaveDialect(dialectName.text, selectedLanguage);

            if (response == null)
            {
                dialectsDropdown.options.Add(new Dropdown.OptionData {text = dialectName.text});

                GetAllDialects();
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "Dialect could not be saved", new Param {Name = nameof(response), Value = response});
            }
        }

        public void DeleteDialect()
        {
            LOGGER.Log(Level.FINE, "Deleting dialect", new Param {Name = nameof(selectedDialect), Value = selectedDialect});
            
            dialectService.DeleteDialect(selectedDialect);

            Dropdown.OptionData optionData = dialectsDropdown.options.Find(x => string.Equals(x.text, selectedDialect.Name));
            dialectsDropdown.options.Remove(optionData);
            
            GetAllDialects();
        }
        
        public List<Dialect> GetAllDialects()
        {
            dialects = dialectService.GetAllDialects();
            return dialects;
        }
        
        public Dialect GetByNameLanguage(string entityName, Language language)
        {
            return dialectService.GetByNameLanguage(entityName, language);
        }
        
        public Dialect GetSelectedDialect()
        {
            return selectedDialect;
        }

        private void PopulateDialects()
        {
            LOGGER.Log(Level.FINE, "Populating dialect dropdown");
            
            foreach (var dialect in dialects)
            {
                dialectsDropdown.options.Add(new Dropdown.OptionData {text = dialect.Name});
            }
        }

        private void SelectedDialect()
        {
            selectedDialect = dialects[dialectsDropdown.value];
            
            LOGGER.Log(Level.FINE, "Dialect selected", new Param {Name = nameof(selectedDialect), Value = selectedDialect});
        }
    }
}