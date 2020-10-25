using System;
using System.Linq;
using System.Reflection;
using SbLogger;
using SbLogger.Levels;
using UI.Impl;
using UnityEngine;
using Utils.Attributes;
using Utils.LogLevels;

namespace Utils
{
    public class Injector : MonoBehaviour
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(Injector), FileService.GetLogPath());
        
        public static void Initialize()
        {
            InitializeLoggers();
            InitializeServices();
        }

        private static void InitializeLoggers()
        {
            Inject(FindObjectOfType<LanguageActions>(), "LOGGER");
            Inject(FindObjectOfType<DialectActions>(), "LOGGER");
            
        }

        private static void InitializeServices()
        {
            Inject(FindObjectOfType<LanguageActions>(), "languageService");
            Inject(FindObjectOfType<DialectActions>(), "dialectService");
        }

        private static void Inject(object baseClass, string fieldName)
        {
            if (baseClass == null)
            {
                LOGGER.Log(Level.SEVERE, "Base class is null", new Param {Name = nameof(baseClass), Value = baseClass});
                return;
            }
            
            LOGGER.Log(InjectionLevel.INJECTION, "Injecting " + fieldName + " into " + baseClass);
            
            FieldInfo fieldInfo = baseClass.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (fieldInfo == null)
            {
                LOGGER.Log(Level.SEVERE, "Field is null");
                return;
            }
            
            InjectAttribute injectType = (InjectAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(InjectAttribute));

            if (injectType != null)
            {
                object injectedClass = fieldInfo.FieldType == typeof(SLogger)
                    ? SLogger.GetLogger(baseClass.ToString(), FileService.GetLogPath())
                    : CreateByTypeName(fieldInfo.FieldType.ToString());

                if (injectedClass != null)
                {
                    fieldInfo.SetValue(baseClass, injectedClass);
                }
                else
                {
                    LOGGER.Log(Level.SEVERE, "The injected class is null");
                }
            }
            else
            {
                LOGGER.Log(Level.SEVERE, "Injection failed for " + fieldName + " into " + baseClass);
            }
        }

        private static object CreateByTypeName(string typeName)
        {
            Type type = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from t in assembly.GetTypes()
                where t.FullName == typeName
                select t).FirstOrDefault();

            if (type != null)
            {
                return Activator.CreateInstance(type);
            }

            LOGGER.Log(Level.SEVERE, "Type " + typeName + " not found");
            return null;
        }
    }
}