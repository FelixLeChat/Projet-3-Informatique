using System.Threading;
using FrontEndAccess.WebsocketAccess;
using Helper.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Communication;
using Models.Token;

namespace PrincessAPITest.SessionEventWebSocketTest
{
    [TestClass]
    public class SessionEventTest : AbstractTest
    {
        private string Token { get; set; }
        private UserToken CurrentUserToken { get; set; }
        private static bool _eventReceived;

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                Token = UserAccess.Register(DefaultUser.Username, DefaultUser.Password);
            }
            catch
            {
                Token = UserAccess.Login(DefaultUser.Username, DefaultUser.Password);
            }
            CurrentUserToken = JwtHelper.DecodeToken(Token);

        }

        [TestCleanup]
        public void Cleanup()
        {
            FrontEndAccess.UserToken.Token = Token;
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void CheckWebsocketUpTest()
        {
            var ws = SessionEventWebsocketAccess.Instance;
            ws.OpenSocket();
            ws.OnSessionEvent += HandleEvent;

            SessionEventWebsocketAccess.Instance.SendMessage(new SessionEventMessage()
            {
                EventType = 0,
                JsonEvent = "---",
                UserToken = CurrentUserToken.Token,
                OnlineSessionId = "---"
            });

            Thread.Sleep(2000);

            Assert.IsTrue(_eventReceived);
        }

        private void HandleEvent(SessionEventMessage message)
        {
            _eventReceived = true;
        }

    }
}
