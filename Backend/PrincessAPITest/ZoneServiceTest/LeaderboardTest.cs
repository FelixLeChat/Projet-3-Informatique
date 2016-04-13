using System;
using FrontEndAccess;
using Helper.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Database;

namespace PrincessAPITest.ZoneServiceTest
{
    [TestClass]
    public class LeaderboardTest : AbstractTest
    {
        private string Token { get; set; }
        private MapModel DefaultZone { get; set; }
        private LeaderModel DefaultLeader { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                Token = UserAccess.Register(DefaultUser.Username, DefaultUser.Password);
            }
            catch (Exception)
            {
                Token = UserAccess.Login(DefaultUser.Username, DefaultUser.Password);
            }

            var userToken = JwtHelper.DecodeToken(Token);

            // Delete all zones
            var zones = ZoneAccess.GetMyZones();
            foreach (var zone in zones)
            {
                ZoneAccess.DeleteMap(zone.HashId);
            }

            DefaultZone = new MapModel()
            {
                Content = "Default Content",
                Level = 0,
                Name = "newZone2134321"
            };

            DefaultLeader = new LeaderModel()
            {
                PlayerHashId = userToken.UserId,
                PlayerName = userToken.Username,
                Points = 100
            };
            DefaultZone.HashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ZoneAccess.DeleteMap(DefaultZone.HashId);
            UserToken.Token = Token;
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void GetTheDefaultLeaderboardTest()
        {
            var leaderboard = LeaderboardAccess.GetLeaderboard(DefaultZone.HashId);
            Assert.AreEqual(DefaultZone.HashId,leaderboard.ZoneHashId);
            var leaders = LeaderboardModelHelper.GetOrderedLeaders(leaderboard);
            Assert.AreEqual(0, leaders.Count);
        }

        [TestMethod]
        public void AddOneLeaderToLeaderboardTest()
        {
            LeaderboardAccess.AddLeaderEntry(DefaultZone.HashId, DefaultLeader);

            var leaderboard = LeaderboardAccess.GetLeaderboard(DefaultZone.HashId);
            var leaders = LeaderboardModelHelper.GetOrderedLeaders(leaderboard);
            Assert.AreEqual(1, leaders.Count);
        }

        [TestMethod]
        public void AddMoreThan10LeadersTest()
        {
            for (int i = 0; i < 12; i++)
            {
                DefaultLeader.Points += 1;
                LeaderboardAccess.AddLeaderEntry(DefaultZone.HashId, DefaultLeader);
            }

            var leaderboard = LeaderboardAccess.GetLeaderboard(DefaultZone.HashId);
            var leaders = LeaderboardModelHelper.GetOrderedLeaders(leaderboard);
            Assert.AreEqual(10, leaders.Count);

            Assert.AreEqual(DefaultLeader.Points,leaders[0].Points);
        }
    }
}
