using System;
using System.Collections.Generic;
using DataBase;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SbLogger;
using ScotchBoardSQL;
using UI.Impl;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.LogLevels;

namespace Editor.Tests.TestCase
{
    public abstract class DoxyTestCase
    {
        private const string BASE_SCENE_NAME = "Assets/Scenes/LanguageTestScene.unity";
        private string[] Scenes = {"LanguageTestScene", "BaseWordTestScene", "DialectTestScene", "DictionaryTestScene"};
        
        protected static SLogger LOGGER;
        
        protected LanguageActions LanguageActions;
        protected DialectActions DialectActions;
        protected BaseWordActions BaseWordActions;
        protected BaseWordActions TranslatedWordActions;
        protected DictionaryActions DictionaryActions;
        
        private DbTrigger dbTrigger;

        private int numberOfTests;
        private readonly Dictionary<TestStatus, int> testStatistics = new Dictionary<TestStatus, int>();
        
        [OneTimeSetUp]
        public void SetUp()
        {
            OpenScene();

            dbTrigger = GameObject.Find("Actions").GetComponent<DbTrigger>();
            
            if (dbTrigger == null)
            {
                throw new NullReferenceException();
            }
            
            dbTrigger.TestAwake();
            DbContext.INSTANCE.ExecuteScript(FileService.ParseFile(FileService.CreateFullPath(Const.ADD_TEST_DATA)).ToString());
            LOGGER = SLogger.GetLogger(nameof(DoxyTestCase), FileService.GetLogPath());
            
            LOGGER.Log(TestLevel.TEST, "============== Setting up test specific");
            SetUpTestSpecific();
            
            LOGGER.Log(TestLevel.TEST, "==================================");
            LOGGER.Log(TestLevel.TEST, "============== Starting Test Suite " + TestContext.CurrentContext.Test.ClassName + "\n");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            LOGGER.Log(TestLevel.TEST, "============== Clearing database");
            
            string deleteAllDictionaries = new Query(Const.SCHEMA, Const.DICTIONARY_TABLE).Delete().Execute();
            DbContext.INSTANCE.ExecuteCommand(deleteAllDictionaries);

            string deleteAllBaseWords = new Query(Const.SCHEMA, Const.BASEWORD_TABLE).Delete().Execute();
            DbContext.INSTANCE.ExecuteCommand(deleteAllBaseWords);
            
            string deleteAllDialects = new Query(Const.SCHEMA, Const.DIALECT_TABLE).Delete().Execute();
            DbContext.INSTANCE.ExecuteCommand(deleteAllDialects);
            
            string deleteAllLanguages = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Delete().Execute();
            DbContext.INSTANCE.ExecuteCommand(deleteAllLanguages);

            LOGGER.Log(TestLevel.TEST, "==================================");
            LOGGER.Log(TestLevel.TEST,
                "============== REPORT Test Suite " + TestContext.CurrentContext.Test.ClassName);
            
            LOGGER.Log(TestLevel.TEST,
                "Number of tests: " + numberOfTests + ";");
            LOGGER.Log(TestLevel.TEST,
                "Successful tests: " +
                (testStatistics.ContainsKey(TestStatus.Passed) ? testStatistics[TestStatus.Passed] : 0) + ";");
            LOGGER.Log(TestLevel.TEST,
                "Failed tests: " +
                (testStatistics.ContainsKey(TestStatus.Failed) ? testStatistics[TestStatus.Failed] : 0) + ";");
            LOGGER.Log(TestLevel.TEST,
                "Skipped tests: " +
                (testStatistics.ContainsKey(TestStatus.Skipped) ? testStatistics[TestStatus.Skipped] : 0) + ";");
            LOGGER.Log(TestLevel.TEST,
                "Inconclusive tests: " +
                (testStatistics.ContainsKey(TestStatus.Inconclusive) ? testStatistics[TestStatus.Inconclusive] : 0) + ";");
            
            LOGGER.Log(TestLevel.TEST,
                "============== Test Suite " + TestContext.CurrentContext.Test.ClassName + " ended");
            LOGGER.Log(TestLevel.TEST, "==================================\n");
        }

        [SetUp]
        public void BeforeTest()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            LOGGER.Log(TestLevel.TEST, "============== " + testName + " started");
        }

        [TearDown]
        public void AfterTest()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var testStatus = string.Empty;
            var finalTestStatus = TestContext.CurrentContext.Result.Outcome.Status;

            switch (finalTestStatus)
            {
                case (TestStatus.Passed):
                    testStatus = " ended successfully";
                    break;
                
                case (TestStatus.Failed):
                    testStatus = " failed";
                    break;
                
                case (TestStatus.Skipped):
                    testStatus = " was skipped";
                    break;
                
                case (TestStatus.Inconclusive):
                    testStatus = " was inconclusive";
                    break;
            }

            numberOfTests++;
            if (!testStatistics.ContainsKey(finalTestStatus))
            {
                testStatistics.Add(finalTestStatus, 1);
            }
            else
            {
                testStatistics[finalTestStatus]++;
            }
            
            LOGGER.Log(TestLevel.TEST, "============== " + testName + testStatus + "\n");
        }

        protected virtual void OpenScene()
        {
            int nr = 0;
            Scene currentScene = SceneManager.GetActiveScene();

            foreach (string scene in Scenes)
            {
                if (currentScene.name.Equals(scene))
                {
                    nr++;
                }
            }

            if (nr == 0)
            {
                EditorSceneManager.OpenScene(BASE_SCENE_NAME);
            }
        }
        protected abstract void SetUpTestSpecific();
    }
}