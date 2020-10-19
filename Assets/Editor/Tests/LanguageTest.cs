using System;
using Editor.Tests.TestCase;
using NUnit.Framework;
using UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Utils.LogLevels;

using static Editor.Tests.TestCase.DoxyBase;

namespace Editor.Tests
{
    public class LanguageTest : DoxyTestCase
    {
        private const string SCENE_NAME = "Assets/Scenes/LanguageTestScene.unity";
        
        private InputField nameInput;
        private Text nameText;
        private Button saveButton;
        private Button getAllButton;
        private Button deleteButton;
        private Dropdown languagesDropdown;

        protected override void OpenScene()
        {
            EditorSceneManager.OpenScene(SCENE_NAME);
        }

        protected override void SetUpTestSpecific()
        {
            LanguageActions = GameObject.Find("Actions").GetComponent<LanguageActions>();
            
            if (LanguageActions == null)
            {
                LOGGER.Log(TestLevel.TEST, "LanguageActions not found");
                throw new NullReferenceException();
            }
            
            LanguageActions.Start();
        }

        [OneTimeSetUp]
        public void TestSuiteSetUp()
        {
            nameInput = GameObject.Find("Name Input").GetComponent<InputField>();
            nameText = GameObject.Find("Name Text").GetComponent<Text>();
            saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
            getAllButton = GameObject.Find("GetAllButton").GetComponent<Button>();
            deleteButton = GameObject.Find("DeleteButton").GetComponent<Button>();
            languagesDropdown = GameObject.Find("Languages").GetComponent<Dropdown>();
            
            saveButton.onClick.AddListener(LanguageActions.SaveLanguage);
        }

        [Test]
        [Order(0)]
        public void LanguagesCount_Test()
        {
            var languages = LanguageActions.GetAllLanguages();
            
            Assertions.AreEqual(languages.Count, languagesDropdown.options.Count);
        }

        [Test]
        [Order(1)]
        public void AddNewLanguage_Test()
        {
            nameInput.text = "German";
            Click(saveButton);

            var germanLanguage = LanguageActions.GetByName(nameInput.text);
            var options = languagesDropdown.options;
            
            Assertions.AreEqual(germanLanguage.Name, options[options.Count - 1].text);
        }

        [Test]
        [Order(2)]
        public void AddExistingLanguage_Test()
        {
            nameInput.text = "German";
            Click(saveButton);
            
            var options = languagesDropdown.options;
            
            Assertions.AreEqual(nameInput.text, options[options.Count - 1].text);
        }

        [Test]
        [Order(3)]
        public void DeleteLanguage_Test()
        {
            var options = languagesDropdown.options;
            languagesDropdown.value = options.Count - 1;

            LanguageActions.DeleteLanguage();  
            Assertions.AreEqual(3, languagesDropdown.options.Count);
        }
    }
}