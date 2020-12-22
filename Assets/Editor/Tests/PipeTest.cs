using Editor.Tests.TestCase;
using NUnit.Framework;

namespace Editor.Tests
{
    public class PipeTest : DoxyTestCase
    {
        protected override void OpenScene()
        {
        }

        protected override void SetUpTestSpecific()
        {
        }
        
        [Test]
        public void PipeDummy_Test()
        {
            Assertions.AreEqual("ceapa", "ceapa");
        }
    }
}
