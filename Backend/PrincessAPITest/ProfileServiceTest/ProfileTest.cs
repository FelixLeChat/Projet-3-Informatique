using System;
using FrontEndAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrincessAPITest.ProfileServiceTests
{
    [TestClass]
    public class ProfileTest : AbstractTest
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
        public void GetUserProfileTest()
        {
            try
            {
                var profile = ProfileAccess.GetUserProfile();

                if(string.IsNullOrWhiteSpace(profile?.AchievementsString) || string.IsNullOrWhiteSpace(profile.StatsString))
                    Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void ModifyUserProfileTest()
        {
            try
            {
                // get profile
                var description = "New Description";
                var profile = ProfileAccess.GetUserProfile();

                // modify profile
                profile.Description = description;
                ProfileAccess.UpdateUserProfile(profile);

                // Check for modifications
                profile = ProfileAccess.GetUserProfile();
                if(profile.Description != description)
                    Assert.Fail("Update failed on user profile");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void GetAllPublicUserInfoTest()
        {
            // get all public user profile
            var firstCount = ProfileAccess.GetAllPublicUserInfos().Count;

            //create new public user
            UserAccess.Register(DefaultUser.Username + "1", DefaultUser.Password);

            var secondCount = ProfileAccess.GetAllPublicUserInfos().Count;
            Assert.AreEqual(firstCount+1, secondCount);

            // not access to private user profile
            var profile = ProfileAccess.GetUserProfile();
            profile.IsPrivate = true;
            ProfileAccess.UpdateUserProfile(profile);

            var thirdCount = ProfileAccess.GetAllPublicUserInfos().Count;
            Assert.AreEqual(firstCount, thirdCount);

            // Delete user
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void SetTutorialFinishedEditorTest()
        {
            ProfileAccess.SetEditorTutorialVisibility(false);

            var profile = ProfileAccess.GetUserProfile();
            Assert.IsFalse(profile.ShowEditorTutorial);
        }

        [TestMethod]
        public void SetTutorialFinishedGameTest()
        {
            ProfileAccess.SetGameTutorialVisibility(false);

            var profile = ProfileAccess.GetUserProfile();
            Assert.IsFalse(profile.ShowGameTutorial);
        }

        [TestMethod]
        public void SetTutorialFinishedLightTest()
        {
            ProfileAccess.SetLightTutorialVisibility(false);

            var profile = ProfileAccess.GetUserProfile();
            Assert.IsFalse(profile.ShowLightTutorial);
        }
    }
}
