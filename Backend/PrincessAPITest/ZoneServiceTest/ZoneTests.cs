using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using FrontEndAccess;
using Helper.Image;
using Helper.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Database;

namespace PrincessAPITest.ZoneServiceTest
{
    [TestClass]
    public class ZoneTests : AbstractTest
    {
        private string Token { get; set; }
        private MapModel DefaultZone { get; set; }

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
        }

        [TestCleanup]
        public void Cleanup()
        {
            UserToken.Token = Token;
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void CreateZoneNoNameTest()
        {
            try
            {
                ZoneAccess.CreateMap("", DefaultZone.Content, DefaultZone.Level);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void CreateZoneNoContentTest()
        {
            try
            {
                ZoneAccess.CreateMap(DefaultZone.Name, "", DefaultZone.Level);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void CreateZoneInvalidLevelTest()
        {
            try
            {
                ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, 1000);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void CreateZoneTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);
            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void UpdateZoneTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, 1);
            ZoneAccess.UpdateMap(mapHashId, DefaultZone.Content, 2);
            var updatedGame = ZoneAccess.GetMapFromId(mapHashId);
            Assert.AreEqual(2, updatedGame.Level);
            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void CreateZoneWithExistingNameTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);
            try
            {
                ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);
                Assert.Fail();
            }
            catch
            {
            }
            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void ListPlayerZonesTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);

            var zones = ZoneAccess.GetMyZones();
            Assert.AreEqual(1, zones.Count);

            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void ListOtherPlayerZonesTest()
        {
            // create new player and one map
            var token = "";
            try
            {
                token = UserAccess.Register(DefaultUser.Username + "1", DefaultUser.Password);
            }
            catch
            {
                token = UserAccess.Login(DefaultUser.Username + "1", DefaultUser.Password);
            }
            var decodedToken = JwtHelper.DecodeToken(token);
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);

            UserToken.Token = Token;
            var maps = ZoneAccess.GetPlayerZones(decodedToken.UserId);
            Assert.AreEqual(1, maps.Count);
            Assert.AreEqual(DefaultZone.Name, maps[0].Name);

            // login as user to delete map and user
            UserToken.Token = token;
            ZoneAccess.DeleteMap(mapHashId);
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void ListFriendsZonesTest()
        {
            // create map
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);
            var decodedToken = JwtHelper.DecodeToken(Token);


            //create new public user
            try
            {
                UserToken.Token = UserAccess.Register(DefaultUser.Username + "1", DefaultUser.Password);
            }
            catch
            {
                UserToken.Token = UserAccess.Login(DefaultUser.Username + "1", DefaultUser.Password);
            }

            var zones = ZoneAccess.GetMyFriendsZones();
            Assert.AreEqual(0, zones.Count);

            // Add user as friend
            var friendsList = new List<BasicUserInfo>
            {
                new BasicUserInfo()
                {
                    HashId = decodedToken.UserId,
                    Username = decodedToken.Username
                }
            };
            FriendAccess.UpdateFriends(friendsList);

            zones = ZoneAccess.GetMyFriendsZones();
            Assert.AreEqual(1, zones.Count);

            // delete new user
            UserAccess.DeleteUser();


            // Delete Map
            UserToken.Token = Token;
            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void ListPublicUsermapsTest()
        {
            var firstCount = ZoneAccess.GetPublicZones().Count;

            // create map
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);
            var myMaps = ZoneAccess.GetMyZones().Count;
            var secondCount = ZoneAccess.GetPublicZones().Count;

            //Assert.AreEqual(firstCount+1, myMaps);
            Assert.AreEqual(firstCount+1, secondCount);

            //create new public user
            try
            {
                UserToken.Token = UserAccess.Register(DefaultUser.Username + "1", DefaultUser.Password);
            }
            catch
            {
                UserToken.Token = UserAccess.Login(DefaultUser.Username + "1", DefaultUser.Password);
            }

            var zones = ZoneAccess.GetPublicZones();
            Assert.AreEqual(secondCount, zones.Count);

            // delete new user
            UserAccess.DeleteUser();


            // Delete Map
            UserToken.Token = Token;
            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void ListPublicuserMapsPrivateMapTest()
        {
            var firstCount = ZoneAccess.GetPublicZones().Count;

            // set private user
            var profile = ProfileAccess.GetUserProfile();

            // modify profile
            profile.IsPrivate = true;
            ProfileAccess.UpdateUserProfile(profile);

            // create map
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);

            //create new public user
            try
            {
                UserToken.Token = UserAccess.Register(DefaultUser.Username + "1", DefaultUser.Password);
            }
            catch
            {
                UserToken.Token = UserAccess.Login(DefaultUser.Username + "1", DefaultUser.Password);
            }

            var zones = ZoneAccess.GetPublicZones();
            Assert.AreEqual(firstCount, zones.Count);

            // delete new user
            UserAccess.DeleteUser();


            // Delete Map
            UserToken.Token = Token;
            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void GetMapFromHashIdTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);
            var map = ZoneAccess.GetMapFromId(mapHashId);

            Assert.AreEqual(DefaultZone.Name, map.Name);
            Assert.AreEqual(DefaultZone.Content, map.Content);
            Assert.AreEqual(JwtHelper.DecodeToken(Token).UserId, map.CreatorhashId);
            Assert.AreEqual(DefaultZone.Level, map.Level);

            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void DeletePlayerZoneTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);

            var zones = ZoneAccess.GetMyZones();
            Assert.AreEqual(1, zones.Count);

            ZoneAccess.DeleteMap(mapHashId);

            zones = ZoneAccess.GetMyZones();
            Assert.AreEqual(0, zones.Count);
        }

        [TestMethod]
        public void AddImageToMapTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);

            // Save Crown to folder
            Properties.Resources.CrownPrincess.Save("princess.png");
            // load image bytes
            var img = Image.FromFile("princess.png");
            var bArr = ImageHelper.ImgToByteArray(img);
            
            ZoneAccess.UpdateMapImage(mapHashId, bArr);
            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void AddImageToMapAlreadyExistTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);

            // Save Crown to folder
            Properties.Resources.CrownPrincess.Save("princess.png");
            // load image bytes
            var img = Image.FromFile("princess.png");
            var bArr = ImageHelper.ImgToByteArray(img);

            Thread.Sleep(1000);
            ZoneAccess.UpdateMapImage(mapHashId, bArr);
            Thread.Sleep(1000);
            ZoneAccess.UpdateMapImage(mapHashId, bArr);
            ZoneAccess.DeleteMap(mapHashId);
        }

        [TestMethod]
        public void GetImageForMapTest()
        {
            var mapHashId = ZoneAccess.CreateMap(DefaultZone.Name, DefaultZone.Content, DefaultZone.Level);

            var path = Path.GetTempPath() + mapHashId + ".png";
            try
            {
                File.Delete(path);
            }
            catch
            {
            }

            Assert.IsFalse(File.Exists(path));
            var filePath = ZoneAccess.GetMapImage(mapHashId);
            Assert.AreEqual(path, filePath);
            Assert.IsTrue(File.Exists(filePath));

            ZoneAccess.DeleteMap(mapHashId);
        }
    }
}
