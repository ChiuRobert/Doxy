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
    [RequireComponent(typeof(IDialectActions))]
    public class BaseWordActions : MonoBehaviour
    {
        [Inject]
        private static SLogger LOGGER;

        [Inject]
        private BaseWordService baseWordService;
        
        private IDialectActions dialectActions;
        
        private List<BaseWord> baseWords;
        private BaseWord selectedBaseWord;
        
        public Dropdown baseWordsDropdown;
        public Text baseWordName;
        
        public void Start()
        {
            LOGGER.Log(Level.INFO, "[3]BaseWordActions initialized");

            dialectActions = GameObject.Find("Actions").GetComponent<IDialectActions>();
            
            GetAllBaseWords();
            PopulateBaseWords();

            baseWordsDropdown.onValueChanged.AddListener(delegate { SelectedBaseWord(); });
        }

        public void SaveBaseWord()
        {
            var selectedDialect = dialectActions.GetSelectedDialect();
            
            LOGGER.Log(Level.FINE, "Saving BaseWord", 
                new Param[]
                {
                    new Param {Name = nameof(baseWordName.text), Value = baseWordName.text},
                    new Param {Name = nameof(selectedDialect), Value = selectedDialect}
                });

            if (selectedDialect == null)
            {
                LOGGER.Log(Level.SEVERE, "BaseWord could not be saved because the selected dialect is null");
                return;
            }
            
            string response = baseWordService.SaveBaseWord(baseWordName.text, selectedDialect);

            if (response == null)
            {
                baseWordsDropdown.options.Add(new Dropdown.OptionData {text = baseWordName.text});

                GetAllBaseWords();
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "BaseWord could not be saved", new Param {Name = nameof(response), Value = response});
            }
        }

        public void DeleteBaseWord()
        {
            LOGGER.Log(Level.FINE, "Deleting BaseWord", new Param {Name = nameof(selectedBaseWord), Value = selectedBaseWord});
            
            baseWordService.DeleteBaseWord(selectedBaseWord);

            Dropdown.OptionData optionData = baseWordsDropdown.options.Find(x => string.Equals(x.text, selectedBaseWord.Word));
            baseWordsDropdown.options.Remove(optionData);
            
            GetAllBaseWords();
        }
        
        public List<BaseWord> GetAllBaseWords()
        {
            baseWords = baseWordService.GetAllBaseWords();
            return baseWords;
        }
        
        public BaseWord GetByWordDialect(string word, Dialect dialect)
        {
            return baseWordService.GetByWordDialect(word, dialect);
        }

        private void PopulateBaseWords()
        {
            LOGGER.Log(Level.FINE, "Populating BaseWords dropdown");
            
            foreach (var baseWord in baseWords)
            {
                baseWordsDropdown.options.Add(new Dropdown.OptionData {text = baseWord.Word});
            }
        }

        private void SelectedBaseWord()
        {
            selectedBaseWord = baseWords[baseWordsDropdown.value];
            
            LOGGER.Log(Level.FINE, "BaseWord selected", new Param {Name = nameof(selectedBaseWord), Value = selectedBaseWord});
        }
    }
}