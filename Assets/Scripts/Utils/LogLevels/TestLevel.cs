using SbLogger.Levels;

namespace Utils.LogLevels
{
    public class TestLevel : Level 
    {
        public static readonly Level TEST = new TestLevel("Test", 800);

        private TestLevel(string name, int level) : base(name, level) { }
    }
}