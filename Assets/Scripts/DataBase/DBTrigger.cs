using SbLogger;
using SbLogger.Levels;
using UnityEngine;
using Utils;

namespace DataBase
{
    public class DbTrigger : MonoBehaviour
    {
        private static SLogger LOGGER;

        public void Awake()
        {
            SetCrossPlatformPath(Database.Production);
            DbContext.INSTANCE.Initialize();
            
            Injector.Initialize();
        }

        public void TestAwake()
        {
            SetCrossPlatformPath(Database.Test);
            DbContext.INSTANCE.Initialize();
            
            Injector.Initialize();
        }

        private static void SetCrossPlatformPath(Database databaseType)
        {
            string path;

#if UNITY_ANDROID
            path = "jar:file://" + Application.dataPath + "!/assets/";
#endif
            
#if UNITY_EDITOR || UNITY_STANDALONE
            path = Application.dataPath + "/StreamingAssets/";
#endif

#if UNITY_EDITOR_LINUX
            path = Application.dataPath + "/Assets/StreamingAssets/";
#endif
            
            DbContext.INSTANCE.DatabaseType = databaseType;
            Const.STREAMING_ASSETS = path;
            
            LOGGER = SLogger.GetLogger(nameof(DbTrigger), FileService.GetLogPath());
            LOGGER.Log(Level.CONFIG, "Initializing game");
            LOGGER.Log(Level.CONFIG, "Path established", new Param { Name = nameof(path), Value = path });
        }
    }
}
