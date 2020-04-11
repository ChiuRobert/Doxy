using System.IO;
using System.Text;
using SbLogger;
using SbLogger.Levels;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Contains services regarding file manipulation
    /// </summary>
    public static class FileService
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(FileService), GetLogPath());

        /// <summary>
        /// Parse the given file
        /// </summary>
        /// <param name="filePath">path of the file to be parsed</param>
        /// <returns>a StringBuilder containing file's content</returns>
        public static StringBuilder ParseFile(string filePath)
        {
            LOGGER.Log(Level.FINE, "Starting to parse file", new Param { Name = nameof(filePath), Value = filePath });

            StringBuilder parser = new StringBuilder();

            if (!File.Exists(filePath))
            {
                LOGGER.Log(Level.SEVERE, "File doesn't exist", new Param { Name = nameof(filePath), Value = filePath });

                return parser;
            }

            StreamReader streamReader = new StreamReader(filePath);
            string fileContents = streamReader.ReadToEnd();
            streamReader.Close();

            string[] lines = fileContents.Split("\n"[0]);

            foreach (var line in lines)
            {
                parser.Append(line);
            }

            LOGGER.Log(Level.FINE, "Finished parsing the file", new Param { Name = nameof(filePath), Value = filePath });

            return parser;
        }

        /// <summary>
        /// Create a file path by taking into consideration the platform
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>A string containing the full path</returns>
        public static string CreateFullPath(string name)
        {
            string path;

#if UNITY_ANDROID
            path = "jar:file://" + Application.dataPath + "!/assets/" + name;
#endif

#if UNITY_EDITOR
            path = Application.dataPath + "/StreamingAssets/" + name;
#endif

            return path;
        }

        /// <summary>
        /// Returns the file path of the log taking into consideration the platform
        /// </summary>
        /// <returns>A string containing the full path</returns>
        /// <see cref="CreateFullPath(string)"/>
        public static string GetLogPath()
        {
            return CreateFullPath(Const.LOG_FILE_NAME);
        }
    }
}