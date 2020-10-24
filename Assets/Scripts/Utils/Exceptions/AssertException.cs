using System;
using SbLogger;
using Utils.LogLevels;

namespace Utils.Exceptions
{
    public class AssertException : Exception
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(AssertException), FileService.GetLogPath());
        
        public AssertException(string message) : base(message)
        {
            LOGGER.Log(TestLevel.TEST_SEVERE, "Assertion failed with message: " + message);
        }
    }
}