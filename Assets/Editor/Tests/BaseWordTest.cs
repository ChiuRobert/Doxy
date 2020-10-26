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
    public class BaseWordTest : DoxyTestCase
    {
        private const string SCENE_NAME = "Assets/Scenes/BaseWordTestScene.unity";
        
        private InputField nameInput;
        private Text nameText;
        private Button saveButton;
        private Button deleteButton;
        private Dropdown languagesDropdown;
        private Dropdown dialectsDropdown;
        private Dropdown baseWordsDropdown;
        
        protected override void OpenScene()
        {
            EditorSceneManager.OpenScene(SCENE_NAME);
        }

        protected override void SetUpTestSpecific()
        {
            LanguageActions = GameObject.Find("Actions").GetComponent<LanguageActions>();
            DialectActions = GameObject.Find("Actions").GetComponent<DialectActions>();
            BaseWordActions = GameObject.Find("Actions").GetComponent<BaseWordActions>();
            
            if (LanguageActions == null)
            {
                LOGGER.Log(TestLevel.TEST_SEVERE, "LanguageActions not found");
                throw new NullReferenceException();
            }
            if (DialectActions == null)
            {
                LOGGER.Log(TestLevel.TEST_SEVERE, "DialectActions not found");
                throw new NullReferenceException();
            }
            if (BaseWordActions == null)
            {
                LOGGER.Log(TestLevel.TEST_SEVERE, "BaseWordActions not found");
                throw new NullReferenceException();
            }
            
            LanguageActions.Start();
            DialectActions.Start();
            BaseWordActions.Start();
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
            languagesDropdown = GameObject.Find("Languages")?.GetComponent<Dropdown>() ??
                                throw new GameObjectNotFoundException("languagesDropdown doesn't exist");
            dialectsDropdown = GameObject.Find("Dialects")?.GetComponent<Dropdown>() ??
                               throw new GameObjectNotFoundException("dialectsDropdown doesn't exist");
            baseWordsDropdown = GameObject.Find("BaseWords")?.GetComponent<Dropdown>() ??
                                throw new GameObjectNotFoundException("baseWordsDropdown doesn't exist");
            
            saveButton.onClick.AddListener(BaseWordActions.SaveBaseWord);
            deleteButton.onClick.AddListener(BaseWordActions.DeleteBaseWord);
        }
        
        [Test]
        [Order(0)]
        public void BaseWordsCount_Test()
        {
            var baseWords = BaseWordActions.GetAllBaseWords();
            
            Assertions.AreEqual(baseWords.Count, baseWordsDropdown.options.Count);
        }
        
        [Test]
        [Order(1)]
        public void AddNewBaseWord_Test()
        {
            // Set what the BaseWord's word will be
            Input(nameInput, "ie");

            // Select the Dialect whose BaseWord this will be
            int hiraganaOption = dialectsDropdown.options.FindIndex(x => string.Equals(x.text, "Hiragana"));
            dialectsDropdown.value = hiraganaOption;
            
            // Save the Dialect
            Click(saveButton);
            
            var options = baseWordsDropdown.options;
            
            Assertions.AreEqual(nameInput.text, options[options.Count - 1].text);
        }

        [Test]
        [Order(2)]
        public void AddSameBaseWordDifferentDialect_Test()
        {
            // Set what the BaseWord's word will be
            Input(nameInput, "ie");

            // Select the Dialect whose BaseWord this will be
            int katakanaOption = dialectsDropdown.options.FindIndex(x => string.Equals(x.text, "Katakana"));
            dialectsDropdown.value = katakanaOption;
            
            // Save the Dialect
            Click(saveButton);
            
            var options = baseWordsDropdown.options;
            
            Assertions.AreEqual(nameInput.text, options[options.Count - 2].text);
            Assertions.AreEqual(nameInput.text, options[options.Count - 1].text);
        }

        [Test]
        [Order(3)]
        public void AddExistingBaseWord_Test()
        {
            Input(nameInput, "ie");
            
            int hiraganaOption = dialectsDropdown.options.FindIndex(x => string.Equals(x.text, "Hiragana"));
            dialectsDropdown.value = hiraganaOption;
            
            Click(saveButton);
            
            var options = baseWordsDropdown.options;
            
            Assertions.AreEqual(nameInput.text, options[options.Count - 1].text);
        }

        [Test]
        [Order(4)]
        public void DeleteBaseWord_Test()
        {
            var options = baseWordsDropdown.options;
            baseWordsDropdown.value = options.Count - 1;

            Click(deleteButton);
            Assertions.AreEqual(9, baseWordsDropdown.options.Count);
        }
    }
}