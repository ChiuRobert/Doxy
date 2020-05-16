using SbLogger.Levels;

namespace Utils.LogLevels
{
    public class InjectionLevel : Level
    {
        public static readonly Level INJECTION = new InjectionLevel("INJECTION", 850);

        public InjectionLevel(string name, int level) : base(name, level) { }
    }
}