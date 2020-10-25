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
    public class DialectTest : DoxyTestCase
    {
        private const string SCENE_NAME = "Assets/Scenes/DialectTestScene.unity";
        
        private InputField nameInput;
        private Text nameText;
        private Button saveButton;
        private Button deleteButton;
        private Dropdown languagesDropdown;
        private Dropdown dialectsDropdown;
        
        protected override void OpenScene()
        {
            EditorSceneManager.OpenScene(SCENE_NAME);
        }

        protected override void SetUpTestSpecific()
        {
            LanguageActions = GameObject.Find("Actions").GetComponent<LanguageActions>();
            DialectActions = GameObject.Find("Actions").GetComponent<DialectActions>();
            
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
            
            LanguageActions.Start();
            DialectActions.Start();
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
            
            saveButton.onClick.AddListener(DialectActions.SaveDialect);
            deleteButton.onClick.AddListener(DialectActions.DeleteDialect);
        }
        
        [Test]
        [Order(0)]
        public void DialectsCount_Test()
        {
            var dialects = DialectActions.GetAllDialects();
            
            Assertions.AreEqual(dialects.Count, dialectsDropdown.options.Count);
        }
        
        [Test]
        [Order(1)]
        public void AddNewDialect_Test()
        {
            // Set what the Dialect's name will be
            Input(nameInput, "Kanji");

            // Select the language whose dialect this will be
            int japaneseOption = languagesDropdown.options.FindIndex(x => string.Equals(x.text, "Japanese"));
            languagesDropdown.value = japaneseOption;
            
            // Save the Dialect
            Click(saveButton);
            
            var options = dialectsDropdown.options;
            
            Assertions.AreEqual(nameInput.text, options[options.Count - 1].text);
        }

        [Test]
        [Order(2)]
        public void AddSameDialectNameDifferentLanguage_Test()
        {
            // Set what the Dialect's name will be
            Input(nameInput, "Kanji");

            // Select the language whose dialect this will be
            int japaneseOption = languagesDropdown.options.FindIndex(x => string.Equals(x.text, "Romanian"));
            languagesDropdown.value = japaneseOption;
            
            // Save the Dialect
            Click(saveButton);
            
            var options = dialectsDropdown.options;
            
            Assertions.AreEqual(nameInput.text, options[options.Count - 2].text);
            Assertions.AreEqual(nameInput.text, options[options.Count - 1].text);
        }

        [Test]
        [Order(3)]
        public void AddExistingDialect_Test()
        {
            Input(nameInput, "Kanji");
            
            int japaneseOption = languagesDropdown.options.FindIndex(x => string.Equals(x.text, "Japanese"));
            languagesDropdown.value = japaneseOption;
            
            Click(saveButton);
            
            var options = dialectsDropdown.options;
            
            Assertions.AreEqual(nameInput.text, options[options.Count - 1].text);
        }

        [Test]
        [Order(4)]
        public void DeleteDialect_Test()
        {
            var options = dialectsDropdown.options;
            dialectsDropdown.value = options.Count - 1;

            Click(deleteButton);
            Assertions.AreEqual(6, dialectsDropdown.options.Count);
        }
    }
}