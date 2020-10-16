using System;
using NUnit.Framework;
using UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Utils.LogLevels;

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

        [Test]
        [Order(0)]
        public void GetRootGameObjects_Test()
        {
            nameInput = GameObject.Find("Name Input").GetComponent<InputField>();
            nameText = GameObject.Find("Name Text").GetComponent<Text>();
            saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
            getAllButton = GameObject.Find("GetAllButton").GetComponent<Button>();
            deleteButton = GameObject.Find("DeleteButton").GetComponent<Button>();
            languagesDropdown = GameObject.Find("Languages").GetComponent<Dropdown>();
        }

        [Test]
        [Order(1)]
        public void LanguagesCount_Test()
        {
            var languages = LanguageActions.GetAllLanguages();
            
            Assert.AreEqual(languages.Count, languagesDropdown.options.Count);
        }

        [Test]
        public void AddLanguage_Test()
        {
            nameInput.text = "German";
            saveButton.onClick.AddListener(() => LanguageActions.SaveLanguage());
            saveButton.onClick.Invoke();

            var germanLanguage = LanguageActions.GetByName(nameInput.text);
            var options = languagesDropdown.options;
            
            Assert.AreEqual(germanLanguage.Name, options[options.Count - 1].text);
        }
    }
}