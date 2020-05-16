using SbLogger.Levels;

namespace Utils.LogLevels
{
    public class DbLevel : Level
    {
        public static readonly Level DB = new DbLevel("DB", 800);

        public DbLevel(string name, int level) : base(name, level) { }
    }
}