using System.Collections.Generic;
using FrontEndAccess;
using Helper.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Database;

namespace PrincessAPITest.ProfileInformationTests
{
    [TestClass]
    public class FriendsTests : AbstractTest
    {
        private string Token { get; set; }
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
        }

        [TestCleanup]
        public void Cleanup()
        {
            UserToken.Token = Token;
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void GetAllFriendsTest()
        {
            var friends = FriendAccess.GetFriends();
            Assert.AreEqual(0, friends.Count);
        }

        [TestMethod]
        public void AddFriendTest()
        {
            var userToken = "";
            try
            {
                userToken = UserAccess.Register(DefaultUser.Username + "1", DefaultUser.Password);
            }
            catch
            {
                userToken = UserAccess.Login(DefaultUser.Username + "1", DefaultUser.Password);
            }
            var decodedToken = JwtHelper.DecodeToken(userToken);

            // Add user as friend
            UserToken.Token = Token;
            var friendsList = new List<BasicUserInfo>
            {
                new BasicUserInfo()
                {
                    HashId = decodedToken.UserId,
                    Username = decodedToken.Username
                }
            };

            FriendAccess.UpdateFriends(friendsList);

            // Verify the friend 
            var friends = FriendAccess.GetFriends();
            Assert.AreEqual(1, friends.Count);

            // delete new user
            UserToken.Token = userToken;
            UserAccess.DeleteUser();
        }
    }
}
