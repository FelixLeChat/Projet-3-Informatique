using System;
using FrontEndAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrincessAPITest.ConnexionServiceTests
{
    [TestClass]
    public class RegistrationTest : AbstractTest
    {
        [TestInitialize]
        public void Initialize()
        {
            try
            {
                // If user already exist, delete him
                UserAccess.Login(DefaultUser.Username, DefaultUser.Password);
                UserAccess.DeleteUser();
            }
            catch
            {
                
            }
        }

        [TestMethod]
        public void RegisterWithValidFacebookUserTest()
        {
            try
            {
                UserAccess.Register(DefaultUser.Username, "", DefaultUser.FacebookId);
                UserAccess.DeleteUser();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void RegisterWithInvalidFacebookUserTest()
        {
            // No Username
            try
            {
                UserAccess.Register("", "", DefaultUser.FacebookId);
                UserAccess.DeleteUser();
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            // No Facebook Id
            try
            {
                UserAccess.Register(DefaultUser.Username, "");
                UserAccess.DeleteUser();
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod]
        public void RegisterWithExistingFacebookUserTest()
        {
            UserToken.Token = UserAccess.Register(DefaultUser.Username, "", DefaultUser.FacebookId);
            try
            {
                UserAccess.Register(DefaultUser.Username, "", DefaultUser.FacebookId);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void RegisterWithValidAppUserTest()
        {
            try
            {
                UserAccess.Register(DefaultUser.Username, DefaultUser.Password);
                UserAccess.DeleteUser();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void RegisterWithInvalidAppUserTest()
        {
            // No Username
            try
            {
                UserAccess.Register("", DefaultUser.Password);
                UserAccess.DeleteUser();
                Assert.Fail();
            }
            catch
            {
            }

            // No Password
            try
            {
                UserAccess.Register(DefaultUser.Username, "");
                UserAccess.DeleteUser();
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void RegisterWithExistingAppUserTest()
        {
            UserAccess.Register(DefaultUser.Username, DefaultUser.Password);
            try
            {
                UserAccess.Register(DefaultUser.Username, DefaultUser.Password);
                Assert.Fail();
            }
            catch
            {
            }
            UserAccess.DeleteUser();
        }
    }
}
