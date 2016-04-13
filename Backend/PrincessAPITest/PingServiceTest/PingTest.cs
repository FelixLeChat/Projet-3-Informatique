using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrincessAPITest.PingServiceTest
{
    [TestClass]
    public class PingTest : AbstractTest
    {
        [TestMethod]
        public void PingTimeTest()
        {
            var time = PingAccess.GetPingMs();
            Assert.IsTrue(time > 0);
        }
    }
}
