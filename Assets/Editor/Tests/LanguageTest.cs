using System;
using Editor.Tests.TestCase;
using NUnit.Framework;
using UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Utils.Exceptions;
using Utils.LogLevels;

using static Editor.Tests.TestCase.DoxyBase;
using AssertionException = UnityEngine.Assertions.AssertionException;

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
            nameInput = GameObject.Find("NameInput")?.GetComponent<InputField>() ??
                        throw new AssertException("nameInput doesn't exist");
            nameText = GameObject.Find("NameText")?.GetComponent<Text>() ??
                       throw new AssertException("nameText doesn't exist");
            saveButton = GameObject.Find("SaveButton")?.GetComponent<Button>() ??
                         throw new AssertException("saveButton doesn't exist");
            getAllButton = GameObject.Find("GetAllButton")?.GetComponent<Button>() ??
                           throw new AssertException("getAllButton doesn't exist");
            deleteButton = GameObject.Find("DeleteButton")?.GetComponent<Button>() ??
                           throw new AssertException("deleteButton doesn't exist");
            languagesDropdown = GameObject.Find("Languages")?.GetComponent<Dropdown>() ??
                                throw new AssertException("languagesDropdown doesn't exist");
            
            saveButton.onClick.AddListener(LanguageActions.SaveLanguage);
            deleteButton.onClick.AddListener(LanguageActions.DeleteLanguage);
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
            Input(nameInput, "German");
            Click(saveButton);

            var germanLanguage = LanguageActions.GetByName(nameInput.text);
            var options = languagesDropdown.options;
            
            Assertions.AreEqual(germanLanguage.Name, options[options.Count - 1].text);
        }

        [Test]
        [Order(2)]
        public void AddExistingLanguage_Test()
        {
            Input(nameInput, "German");
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

            Click(deleteButton);
            Assertions.AreEqual(3, languagesDropdown.options.Count);
        }
    }
}