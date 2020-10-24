using SbLogger.Levels;

namespace Utils.LogLevels
{
    public class TestLevel : Level 
    {
        public static readonly Level TEST = new TestLevel("TEST", 800);
        
        public static readonly Level TEST_SEVERE = new TestLevel("TEST - SEVERE", 900);

        private TestLevel(string name, int level) : base(name, level) { }
    }
}