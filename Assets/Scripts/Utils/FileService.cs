using System.IO;
using System.Text;
using UnityEngine;

namespace Doxy.Utils
{
    /// <summary>
    /// Contains services regarding file manipulation
    /// </summary>
    public static class FileService
    {
        /// <summary>
        /// Parse the given file
        /// </summary>
        /// <param name="filePath">path of the file to be parsed</param>
        /// <returns>a StringBuilder containing file's content</returns>
        public static StringBuilder ParseFile(string filePath)
        {
            StringBuilder parser = new StringBuilder();

            if (!File.Exists(filePath))
            {
                WriteLog("File doesn't exist " + filePath);
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

            return parser;
        }

        /// <summary>
        /// Write the message to a log file
        /// </summary>
        /// <param name="message">message to be written</param>
        public static void WriteLog(string message)
        {
            string path = CreateFullPath(Const.LOG_FILE_NAME);

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(message);
            }
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
    }
}