using System;
using System.Collections.Generic;
using Helper.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Database;
using Models.Token;

namespace PrincessAPITest.GameServiceTest
{
    [TestClass]
    public class GameTest : AbstractTest
    {
        private string Token { get; set; }
        private string OtherUserToken { get; set; }
        private UserToken CurrentUserToken { get; set; }
        private GameModel DefaultGame { get; set; }
        private GameModel OtherPlayerGame { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                OtherUserToken = UserAccess.Register(DefaultUser.Username + "2", DefaultUser.Password);
            }
            catch (Exception)
            {
                OtherUserToken = UserAccess.Login(DefaultUser.Username + "2", DefaultUser.Password);
            }

            try
            {
                Token = UserAccess.Register(DefaultUser.Username, DefaultUser.Password);
            }
            catch
            {
                Token = UserAccess.Login(DefaultUser.Username, DefaultUser.Password);
            }
            CurrentUserToken = JwtHelper.DecodeToken(Token);
            DefaultGame = new GameModel()
            {
                Name = "Default new game",
                MaxPlayersCount = 3,
                IsPrivate = false,
                ZonesHashId = new List<string>() { "testId" }
            };

            OtherPlayerGame = new GameModel()
            {
                Name = "Defaulst new game",
                MaxPlayersCount = 3,
                IsPrivate = false,
                ZonesHashId = new List<string>() { "testId" }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            FrontEndAccess.UserToken.Token = Token;
            UserAccess.DeleteUser();

            FrontEndAccess.UserToken.Token = OtherUserToken;
            UserAccess.DeleteUser();
        }

        [TestMethod]
        public void GetAllGameTest()
        {
            var firstCount = GameAccess.GetAllGames().Count;

            // create game
            var game = GameAccess.CreateGame(DefaultGame);

            var secondCount = GameAccess.GetAllGames().Count;
            Assert.AreEqual(firstCount + 1, secondCount);

            // delete game
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void GetInfoOnGameTest()
        {
            // create game
            var game = GameAccess.CreateGame(DefaultGame);
            var gameInfo = GameAccess.GetGameInfo(game.HashId);
            Assert.AreEqual(game.ParticipantsHashId[0], gameInfo.ParticipantsHashId[0]);
        }

        [TestMethod]
        public void DeleteGameTest()
        {
            var firstCount = GameAccess.GetAllGames().Count;
            // create game
            var game = GameAccess.CreateGame(DefaultGame);
            GameAccess.DeleteGame(game.HashId);
            var secondCount = GameAccess.GetAllGames().Count;

            Assert.AreEqual(firstCount, secondCount);
        }

        [TestMethod]
        public void DeleteInvalidGameTest()
        {
            var game = GameAccess.CreateGame(DefaultGame);
            var oldGamehash = game.HashId;
            try
            {
                game.HashId += "1";
                GameAccess.DeleteGame(game.HashId);
                Assert.Fail();
            }
            catch
            {
                game.HashId = oldGamehash;
                GameAccess.DeleteGame(game.HashId);
            }
        }

        [TestMethod]
        public void DeleteOtherPlayerGameTest()
        {
            // create new user and game
            var userToken = "";
            try
            {
                userToken = UserAccess.Register(DefaultUser.Username + "1", DefaultUser.Password);
            }
            catch
            {
                userToken = UserAccess.Login(DefaultUser.Username + "1", DefaultUser.Password);
            }

            UserAccess.DeleteUser();
            var game = GameAccess.CreateGame(DefaultGame);

            try
            {
                // login as default user
                UserAccess.Login(DefaultUser.Username, DefaultUser.Password);
                GameAccess.DeleteGame(game.HashId);
                Assert.Fail();
            }
            catch
            {
                // login as good user to delete game
                FrontEndAccess.UserToken.Token = userToken;
                GameAccess.DeleteGame(game.HashId);
            }
        }

        [TestMethod]
        public void CreateGameTest()
        {
            // create a basic game
            var game = GameAccess.CreateGame(DefaultGame);

            Assert.AreEqual(JwtHelper.DecodeToken(Token).UserId, game.HostHashId);
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void CreateGameParametersTest()
        {
            // create a basic game
            var game = GameAccess.CreateGame(DefaultGame.Name, DefaultGame.MaxPlayersCount, DefaultGame.ZonesHashId);
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void CreatePrivateGameParametersTest()
        {
            // create a basic game
            var game = GameAccess.CreateGame(DefaultGame.Name, DefaultGame.MaxPlayersCount, DefaultGame.ZonesHashId, "1");
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void CreatePrivateGameNoPassTest()
        {
            // create a private game with no password
            DefaultGame.IsPrivate = true;
            try
            {
                GameAccess.CreateGame(DefaultGame);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void CreatePrivateGameWithPassTest()
        {
            // create a private game with password
            DefaultGame.Password = "1";
            DefaultGame.IsPrivate = true;
            var game = GameAccess.CreateGame(DefaultGame);
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void CreateGameTooManyPlayerTest()
        {
            // create a game with too much player
            try
            {
                DefaultGame.MaxPlayersCount = 10;
                GameAccess.CreateGame(DefaultGame);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void CreateGameNoPlayerTest()
        {
            // create a game with too much player
            try
            {
                DefaultGame.MaxPlayersCount = 0;
                GameAccess.CreateGame(DefaultGame);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void JoinEmptyStringGameTest()
        {
            try
            {
                GameAccess.JoinGame("");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void JoinNonExistingGameTest()
        {
            try
            {
                GameAccess.JoinGame("asd9d0sa00dasmmdsq");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void JoinAGameYouAlreadyAreInTest()
        {
            var game = GameAccess.CreateGame(DefaultGame);
            try
            {
                GameAccess.JoinGame(game.HashId);
                Assert.Fail();
            }
            catch
            {
            }

            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void JoinPrivateGameNoPassTest()
        {
            // login as another user to create a new game
            FrontEndAccess.UserToken.Token = OtherUserToken;
            DefaultGame.IsPrivate = true;
            DefaultGame.Password = "1";
            var game = GameAccess.CreateGame(DefaultGame);

            try
            {
                // login as default user to join
                FrontEndAccess.UserToken.Token = Token;
                GameAccess.JoinGame(game.HashId);
                Assert.Fail();
            }
            catch
            {
            }

            FrontEndAccess.UserToken.Token = OtherUserToken;
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void JoinPrivateGameWrongPassTest()
        {
            FrontEndAccess.UserToken.Token = OtherUserToken;
            DefaultGame.IsPrivate = true;
            DefaultGame.Password = "1";
            var game = GameAccess.CreateGame(DefaultGame);

            try
            {
                FrontEndAccess.UserToken.Token = Token;
                GameAccess.JoinGame(game.HashId, DefaultGame.Password + "1");
                Assert.Fail();
            }
            catch
            {
            }
            FrontEndAccess.UserToken.Token = OtherUserToken;
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void JoinFullGameTest()
        {
            FrontEndAccess.UserToken.Token = OtherUserToken;
            DefaultGame.MaxPlayersCount = 1;
            var game = GameAccess.CreateGame(DefaultGame);

            try
            {
                FrontEndAccess.UserToken.Token = Token;
                GameAccess.JoinGame(game.HashId);
                Assert.Fail();
            }
            catch
            {
            }
            FrontEndAccess.UserToken.Token = OtherUserToken;
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void JoinGameTest()
        {
            FrontEndAccess.UserToken.Token = OtherUserToken;
            var game = GameAccess.CreateGame(DefaultGame);

            FrontEndAccess.UserToken.Token = Token;
            var joinedGame = GameAccess.JoinGame(game.HashId);

            Assert.AreEqual(game.HashId, joinedGame.HashId);
            Assert.AreEqual(2, joinedGame.CurrentPlayerCount);
            Assert.IsTrue(joinedGame.ParticipantsHashId.Contains(CurrentUserToken.UserId));

            FrontEndAccess.UserToken.Token = OtherUserToken;
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void JoinPrivateGameTest()
        {
            FrontEndAccess.UserToken.Token = OtherUserToken;
            DefaultGame.IsPrivate = true;
            DefaultGame.Password = "1";
            var game = GameAccess.CreateGame(DefaultGame);

            FrontEndAccess.UserToken.Token = Token;
            var joinedGame = GameAccess.JoinGame(game.HashId, DefaultGame.Password);

            Assert.AreEqual(game.HashId, joinedGame.HashId);
            Assert.AreEqual(2, joinedGame.CurrentPlayerCount);
            Assert.IsTrue(joinedGame.ParticipantsHashId.Contains(CurrentUserToken.UserId));

            FrontEndAccess.UserToken.Token = OtherUserToken;
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void StartGameNoGameHashTest()
        {
            try
            {
                GameAccess.StartGame("");
            }
            catch
            {
            }
        }

        [TestMethod]
        public void StartGameWrongGameHashTest()
        {
            try
            {
                GameAccess.StartGame("sdd9d8s88asd9dmas9d");
            }
            catch
            {
            }
        }

        [TestMethod]
        public void StartGameNotHostTest()
        {
            // create a basic game
            var game = GameAccess.CreateGame(DefaultGame);
            try
            {
                FrontEndAccess.UserToken.Token = OtherUserToken;
                GameAccess.StartGame(game.HashId);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            FrontEndAccess.UserToken.Token = Token;
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void StartGameTest()
        {
            var game = GameAccess.CreateGame(DefaultGame);
            GameAccess.StartGame(game.HashId);
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void StartStartedGameTest()
        {
            var game = GameAccess.CreateGame(DefaultGame);
            GameAccess.StartGame(game.HashId);
            try
            {
                GameAccess.StartGame(game.HashId);
                Assert.Fail();
            }
            catch
            {
            }
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void QuitGameNotExistTest()
        {
            var game = GameAccess.CreateGame(DefaultGame);
            try
            {
                GameAccess.QuitGame(game.HashId + 1);
                Assert.Fail();
            }
            catch { }
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void QuitgameHostTest()
        {
            var firstCount = GameAccess.GetAllGames().Count;
            var game = GameAccess.CreateGame(DefaultGame);

            GameAccess.QuitGame(game.HashId);

            var secondCount = GameAccess.GetAllGames().Count;
            Assert.AreEqual(firstCount, secondCount);
        }

        [TestMethod]
        public void QuitGameTest()
        {
            var firstCount = GameAccess.GetAllGames().Count;
            FrontEndAccess.UserToken.Token = OtherUserToken;
            var game = GameAccess.CreateGame(DefaultGame);

            FrontEndAccess.UserToken.Token = Token;
            GameAccess.JoinGame(game.HashId);
            GameAccess.QuitGame(game.HashId);

            game = GameAccess.GetGameInfo(game.HashId);
            Assert.AreEqual(1, game.CurrentPlayerCount);
            Assert.AreEqual(1, game.ParticipantsHashId.Count);

            var secondCount = GameAccess.GetAllGames().Count;
            Assert.AreEqual(firstCount + 1, secondCount);

            FrontEndAccess.UserToken.Token = OtherUserToken;
            GameAccess.DeleteGame(game.HashId);
        }

        [TestMethod]
        public void DisconnectAndReconnectTest()
        {
            FrontEndAccess.UserToken.Token = Token;
            var game = GameAccess.CreateGame(DefaultGame);

            try
            {
                //Other player
                FrontEndAccess.UserToken.Token = OtherUserToken;
                GameAccess.JoinGame(game.HashId);

                // Since the game is not starter should not be puit  disconnect
                GameAccess.QuitGame(game.HashId);
                game = GameAccess.GetGameInfo(game.HashId);
                Assert.AreEqual(1, game.CurrentPlayerCount);
                Assert.AreEqual(1, game.ParticipantsHashId.Count);
                Assert.AreEqual(0, game.DisconnectedHashId.Count);

                //Other player join again
                GameAccess.JoinGame(game.HashId);

                // Host start the game
                FrontEndAccess.UserToken.Token = Token;
                GameAccess.StartGame(game.HashId);

                //Other player disconnect
                FrontEndAccess.UserToken.Token = OtherUserToken;
                GameAccess.QuitGame(game.HashId);

                // Should be in the disconnect list
                game = GameAccess.GetGameInfo(game.HashId);
                Assert.AreEqual(1, game.CurrentPlayerCount);
                Assert.AreEqual(1, game.ParticipantsHashId.Count);
                Assert.AreEqual(1, game.DisconnectedHashId.Count);

                // SHould be able to reconnect
                GameAccess.ReconnectGame(game.HashId);
                game = GameAccess.GetGameInfo(game.HashId);
                Assert.AreEqual(2, game.CurrentPlayerCount);
                Assert.AreEqual(2, game.ParticipantsHashId.Count);
                Assert.AreEqual(0, game.DisconnectedHashId.Count);
               
            }
            catch (Exception)
            {
            }
            finally
            {
                FrontEndAccess.UserToken.Token = Token;
                GameAccess.DeleteGame(game.HashId);
            }
        }

        [TestMethod]
        public void SpectateTest()
        {
            FrontEndAccess.UserToken.Token = Token;
            var game = GameAccess.CreateGame(DefaultGame);

            // login as another user to spectate the game
            FrontEndAccess.UserToken.Token = OtherUserToken;

            game = GameAccess.Spectate(game.HashId);

            Assert.AreEqual(1, game.ParticipantsHashId.Count);
            Assert.AreEqual(1, game.SpectatorsHashId.Count);

            GameAccess.QuitGame(game.HashId);

            game = GameAccess.GetGameInfo(game.HashId);
            Assert.AreEqual(1, game.ParticipantsHashId.Count);
            Assert.AreEqual(0, game.SpectatorsHashId.Count);

            FrontEndAccess.UserToken.Token = Token;
            GameAccess.DeleteGame(game.HashId);
        }


    }
}
