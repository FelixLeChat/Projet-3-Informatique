using System;
using FrontEndAccess;
using Helper.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Database;

namespace PrincessAPITest.ConnexionServiceTests
{
    [TestClass]
    public class LoginTest : AbstractTest
    {
        private UserModel FacebookUser { get; set; }
        private string FacebookUserToken { get; set; }

        private UserModel AppUser { get; set; }
        private string AppUserToken { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            FacebookUser = new UserModel() {Username = DefaultUser.Username+"facebook", FacebookId = DefaultUser.FacebookId};
            FacebookUserToken = UserAccess.Register(FacebookUser.Username, "", FacebookUser.FacebookId);

            AppUser = new UserModel() {Username = DefaultUser.Username+"app", Password = DefaultUser.Password};
            AppUserToken = UserAccess.Register(AppUser.Username, AppUser.Password);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Delete facebook user
            UserToken.Token = FacebookUserToken;
            UserAccess.DeleteUser();

            // Delete App user
            UserToken.Token = AppUserToken;
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void LoginWithExistingFacebookUserTest()
        {
            try
            {
                var tokenString = UserAccess.Login(FacebookUser.Username, "", FacebookUser.FacebookId);
                var token = JwtHelper.DecodeToken(tokenString);

                if(string.IsNullOrWhiteSpace(token?.Username))
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void LoginWithNonExistingFacebookuserTest()
        {
            try
            {
                UserAccess.Login(FacebookUser.Username+"1", "", FacebookUser.FacebookId);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod]
        public void LoginWithExistingAppUserTest()
        {
            try
            {
                var tokenString = UserAccess.Login(AppUser.Username, AppUser.Password);
                var token = JwtHelper.DecodeToken(tokenString);

                if (string.IsNullOrWhiteSpace(token?.Username))
                    Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void LoginWithNonExistingAppUserTest()
        {
            try
            {
                UserAccess.Login(AppUser.Username+"1", AppUser.Password);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }
    }
}
