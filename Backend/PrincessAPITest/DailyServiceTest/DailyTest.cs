using FrontEndAccess;
using Helper.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrincessAPITest.DailyServiceTest
{
    [TestClass]
    public class DailyTest : AbstractTest
    {
        private string Token { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            Token = UserAccess.Register(DefaultUser.Username, DefaultUser.Password);
        }

        [TestCleanup]
        public void Cleanup()
        {
            UserToken.Token = Token;
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void GetDailyTest()
        {
            var daily = DailyAccess.GetDaily();
            var userToken = JwtHelper.DecodeToken(Token);

            Assert.IsFalse(daily.IsDone);
            Assert.AreEqual(userToken.UserId, daily.UserHashId);
        }

        [TestMethod]
        public void FinishDailyTest()
        {
            var daily = DailyAccess.GetDaily();
            Assert.IsFalse(daily.IsDone);

            DailyAccess.CompleteDaily();

            daily = DailyAccess.GetDaily();
            Assert.IsTrue(daily.IsDone);
        }

    }
}