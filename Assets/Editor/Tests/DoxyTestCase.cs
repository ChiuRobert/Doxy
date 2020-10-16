using System;
using DataBase;
using NUnit.Framework;
using SbLogger;
using ScotchBoardSQL;
using UI;
using UnityEngine;
using Utils;
using Utils.LogLevels;

namespace Editor.Tests
{
    public class DoxyTestCase
    {
        protected static SLogger LOGGER;
        protected LanguageActions LanguageActions;
        
        private DbTrigger dbTrigger;
        
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
        }

        protected virtual void OpenScene() { }
        protected virtual void SetUpTestSpecific() { }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            LOGGER.Log(TestLevel.TEST, "============== Clearing database");
            
            string deleteAllLanguages = new Query(Const.SCHEMA, Const.LANGUAGE_TABLE).Delete().Execute();
            DbContext.INSTANCE.ExecuteCommand(deleteAllLanguages);
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
            LOGGER.Log(TestLevel.TEST, "============== " + testName + " ended");
        }
    }
}