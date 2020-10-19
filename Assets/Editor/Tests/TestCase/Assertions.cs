using System;
using NUnit.Framework;
using SbLogger;
using Utils;
using Utils.LogLevels;

namespace Editor.Tests.TestCase
{
    public class Assertions : Assert
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(Assertions), FileService.GetLogPath());
        
        public static void AreEqual(double expected, double actual)
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (Exception exception)
            {
                LOGGER.Log(TestLevel.TEST, "Assertion failed: expected [" + expected + "] but was [" + actual + "]\n" +
                                           "Error " + exception);
                Fail(exception.Message);
            }
        }
        
        public static void AreEqual(string expected, string actual)
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (Exception exception)
            {
                LOGGER.Log(TestLevel.TEST, "Assertion failed: expected [" + expected + "] but was [" + actual + "]\n" +
                                           "Error " + exception);
                Fail(exception.Message);
            }
        }
    }
}