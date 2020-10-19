using SbLogger;
using UnityEngine.UI;
using Utils;
using Utils.LogLevels;

namespace Editor.Tests.TestCase
{
    public static class DoxyBase
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(DoxyBase), FileService.GetLogPath());
        
        internal static void Click(Button button)
        {
            LOGGER.Log(TestLevel.TEST, "Starting click on " + button.name);
            button.onClick.Invoke();
            LOGGER.Log(TestLevel.TEST, "Click ended on " + button.name);
        }
    }
}