using System;
using System.Linq;
using System.Reflection;
using SbLogger;
using SbLogger.Levels;
using UI;
using UnityEngine;
using Utils.LogLevels;

namespace Utils
{
    public class Injector : MonoBehaviour
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(Injector), FileService.GetLogPath());
        
        public static void Initialize()
        {
            Inject(FindObjectOfType<LanguageActions>(), "languageService");
            Inject(FindObjectOfType<LanguageActions>(), "LOGGER");
        }

        private static void Inject(object baseClass, string fieldName)
        {
            LOGGER.Log(InjectionLevel.INJECTION, "Injecting " + fieldName + " into " + baseClass);
            
            FieldInfo fieldInfo = baseClass.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            InjectAttribute injectType = (InjectAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(InjectAttribute));

            if (injectType != null)
            {
                object injectedClass = null;

                injectedClass = fieldInfo.FieldType == typeof(SLogger)
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