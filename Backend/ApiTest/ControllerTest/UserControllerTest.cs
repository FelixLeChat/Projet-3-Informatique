using System;
using FrontEndAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Database;

namespace ApiTest.ControllerTest
{
    [TestClass]
    public class UserControllerTest
    {
        private const string Endpoint = "http://localhost:51216";
        private UserAccess _userAccess;
        private UserModel _defaultUser;
        private string _userToken;

        // Appel sur l'api 
        [TestInitialize]
        public void Initialize()
        {
            _userAccess = new UserAccess(Endpoint);
            _defaultUser = new UserModel()
            {
                FacebookId = "ad8c0aca0-9d0-ad-a0mxa-mxaxad",
                Username = "a9sds8d1818dndamsd",
                Password = "1234"
            };

            try
            {
                // if user exist, delete it
                _userAccess.UserToken = _userAccess.Login(_defaultUser.Username, _defaultUser.Password, "");
                _userAccess.DeleteUser();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod]
        public void RegisterTest()
        {
            // register only with credentials
            var result = _userAccess.Register(_defaultUser.Username, _defaultUser.Password, "");

            _userAccess.UserToken = result;
            _userAccess.DeleteUser();
        }
    }
}
