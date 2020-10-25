using System;
using SbLogger;
using Utils.LogLevels;

namespace Utils.Exceptions
{
    public class GameObjectNotFoundException : Exception
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(GameObjectNotFoundException), FileService.GetLogPath());
        
        public GameObjectNotFoundException(string message) : base(message)
        {
            LOGGER.Log(TestLevel.TEST_SEVERE, "GameObject was not found: " + message);
        }
    }
}