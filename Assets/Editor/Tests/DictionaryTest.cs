using System;
using Editor.Tests.TestCase;
using NUnit.Framework;
using UI.Impl;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Utils.Exceptions;
using Utils.LogLevels;

using static Editor.Tests.TestCase.DoxyBase;

namespace Editor.Tests
{
    public class DictionaryTest : DoxyTestCase
    {
        private const string SCENE_NAME = "Assets/Scenes/DictionaryTestScene.unity";
        
        private InputField nameInput;
        private Text nameText;
        private Button saveButton;
        private Button deleteButton;
        private Dropdown dictionaryDropdown;
        private Dropdown baseWordsDropdown;
        private Dropdown translatedWordsDropdown;
        
        protected override void OpenScene()
        {
            EditorSceneManager.OpenScene(SCENE_NAME);
        }

        protected override void SetUpTestSpecific()
        {
            DictionaryActions = GameObject.Find("Actions").GetComponent<DictionaryActions>();
            BaseWordActions[] iBaseWordActions = GameObject.Find("Actions").GetComponents<BaseWordActions>();

            BaseWordActions = iBaseWordActions[0];
            TranslatedWordActions = iBaseWordActions[1];
    
            if (BaseWordActions == null)
            {
                LOGGER.Log(TestLevel.TEST_SEVERE, "BaseWordActions not found");
                throw new NullReferenceException();
            }
            if (TranslatedWordActions == null)
            {
                LOGGER.Log(TestLevel.TEST_SEVERE, "TranslatedWordActions not found");
                throw new NullReferenceException();
            }
            if (DictionaryActions == null)
            {
                LOGGER.Log(TestLevel.TEST_SEVERE, "DictionaryActions not found");
                throw new NullReferenceException();
            }
            
            BaseWordActions.Start();
            TranslatedWordActions.Start();
            DictionaryActions.Start();
        }
        
        [OneTimeSetUp]
        public void TestSuiteSetUp()
        {
            nameInput = GameObject.Find("NameInput")?.GetComponent<InputField>() ??
                        throw new GameObjectNotFoundException("nameInput doesn't exist");
            nameText = GameObject.Find("NameText")?.GetComponent<Text>() ??
                       throw new GameObjectNotFoundException("nameText doesn't exist");
            saveButton = GameObject.Find("SaveButton")?.GetComponent<Button>() ??
                         throw new GameObjectNotFoundException("saveButton doesn't exist");
            deleteButton = GameObject.Find("DeleteButton")?.GetComponent<Button>() ??
                           throw new GameObjectNotFoundException("deleteButton doesn't exist");
            dictionaryDropdown = GameObject.Find("Dictionaries")?.GetComponent<Dropdown>() ??
                                 throw new GameObjectNotFoundException("dictionaryDropdown doesn't exist");
            baseWordsDropdown = GameObject.Find("BaseWords")?.GetComponent<Dropdown>() ??
                                throw new GameObjectNotFoundException("baseWordsDropdown doesn't exist");
            translatedWordsDropdown = GameObject.Find("TranslatedBaseWords")?.GetComponent<Dropdown>() ??
                                      throw new GameObjectNotFoundException("translatedWordsDropdown doesn't exist");
            
            saveButton.onClick.AddListener(DictionaryActions.SaveDictionary);
            deleteButton.onClick.AddListener(DictionaryActions.DeleteDictionary);
        }
        
        [Test]
        [Order(0)]
        public void DictionaryCount_Test()
        {
            var dictionaries = DictionaryActions.GetAllDictionaries();

                Assertions.AreEqual(dictionaries.Count, dictionaryDropdown.options.Count);
        }
        
        [Test]
        [Order(1)]
        public void AddNewDictionary_Test()
        {
            // Select the BaseWord
            int baseWordOption = baseWordsDropdown.options.FindLastIndex(x => string.Equals(x.text, "up"));
            baseWordsDropdown.value = baseWordOption;
            
            // Select the TranslatedWord
            int translatedWordOption = translatedWordsDropdown.options.FindIndex(x => string.Equals(x.text, "sus"));
            translatedWordsDropdown.value = translatedWordOption;
            
            // Save the Dialect
            Click(saveButton);
            
            var options = dictionaryDropdown.options;
            
            Assertions.AreEqual("up = sus", options[options.Count - 1].text);
        }

        [Test]
        [Order(2)]
        public void AddSameBaseWordDifferentTranslateDictionary_Test()
        {
            // Select the BaseWord
            int baseWordOption = baseWordsDropdown.options.FindIndex(x => string.Equals(x.text, "up"));
            baseWordsDropdown.value = baseWordOption;
            
            // Select the TranslatedWord
            int translatedWordOption = translatedWordsDropdown.options.FindLastIndex(x => string.Equals(x.text, "up"));
            translatedWordsDropdown.value = translatedWordOption;
            
            // Save the Dialect
            Click(saveButton);
            
            var options = dictionaryDropdown.options;
            
            Assertions.AreEqual("up = sus", options[options.Count - 2].text);
            Assertions.AreEqual("up = up", options[options.Count - 1].text);
        }

        [Test]
        [Order(3)]
        public void AddExistingDictionary_Test()
        {
            // Select the BaseWord
            int baseWordOption = baseWordsDropdown.options.FindIndex(x => string.Equals(x.text, "up"));
            baseWordsDropdown.value = baseWordOption;
            
            // Select the TranslatedWord
            int translatedWordOption = translatedWordsDropdown.options.FindLastIndex(x => string.Equals(x.text, "up"));
            translatedWordsDropdown.value = translatedWordOption;
            
            Click(saveButton);
            
            var options = dictionaryDropdown.options;
            
            Assertions.AreEqual("up = up", options[options.Count - 1].text);
        }

        [Test]
        [Order(4)]
        public void DeleteDictionary_Test()
        {
            var options = dictionaryDropdown.options;
            dictionaryDropdown.value = options.Count - 1;

            Click(deleteButton);
            Assertions.AreEqual(8, dictionaryDropdown.options.Count);
        }
    }
}