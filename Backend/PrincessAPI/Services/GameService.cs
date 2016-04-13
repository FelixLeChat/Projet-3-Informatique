using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Helper.Http;
using HttpHelper.Hash;
using Models;
using Models.Communication;
using Models.Database;
using Models.Token;
using PrincessAPI.Websocket;
using static Models.Database.GameModelHelper;

namespace PrincessAPI.Services
{
    public class GameService : AbstractService
    {
        private static List<GameModel> GameList = new List<GameModel>();
        private readonly static object LockObj = new object();

        public GameService(UserToken userToken) : base(userToken)
        {
        }

        /// <summary>
        /// Get all public information on game currently hosted
        /// </summary>
        /// <returns></returns>
        public List<GameModel> GetAllGames()
        {
            lock (LockObj)
            {
                return ToPublic(GameList);
            }
        }

        /// <summary>
        /// Get all public info on a certain game
        /// </summary>
        /// <param name="gameHashId"></param>
        /// <returns></returns>
        public GameModel GetGameInfo(string gameHashId)
        {
            if (string.IsNullOrWhiteSpace(gameHashId))
                throw HttpResponseExceptionHelper.Create("You must specify a game hash id",
                    HttpStatusCode.BadRequest);

            // Check if game exist
            lock (LockObj)
            {
                var game = GameList.FirstOrDefault(x => x.HashId == gameHashId);
                if (game == null)
                    throw HttpResponseExceptionHelper.Create("No game exist with specified hash",
                        HttpStatusCode.BadRequest);

                return ToPublic(game);
            }
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public GameModel CreateGame(GameModel game)
        {
            if (game == null || !IsValid(game))
                throw HttpResponseExceptionHelper.Create("Invalid format for specified new Game",
                    HttpStatusCode.BadRequest);

            var username = UserToken.Username;
            game.HostHashId = UserToken.UserId;
            game.HashId = Sha1Hash.GetSha1HashData(username + game.Name);

            // Add host to participants
            game.CurrentPlayerCount = 1;
            game.ParticipantsHashId.Add(UserToken.UserId);
            game.State = EnumsModel.GameState.Waiting;

            // insert in gameList
            lock (LockObj)
            {
                GameList.Add(game);
                return ToPublic(game);
            }
        }

        /// <summary>
        /// Delete the game from the game list
        /// </summary>
        /// <param name="gameHashId"></param>
        public void DeleteGame(string gameHashId)
        {
            if (string.IsNullOrWhiteSpace(gameHashId))
                throw HttpResponseExceptionHelper.Create("You must specify a game id to delete",
                    HttpStatusCode.BadRequest);

            // Check if game exist

            var game = GameList.FirstOrDefault(x => x.HashId == gameHashId);
            if (game == null)
                throw HttpResponseExceptionHelper.Create("No game exist with specified hash to delete",
                    HttpStatusCode.BadRequest);

            if (game.HostHashId != UserToken.UserId)
                throw HttpResponseExceptionHelper.Create(
                    "You are not the creator of the game, you don't have the permission to delete it",
                    HttpStatusCode.BadRequest);

            // remove game from websocket handling game

            var gameItem = GameWebsocket.Games.FirstOrDefault(x => x.HashId == gameHashId);
            if (gameItem != null)
                GameWebsocket.Games.Remove(gameItem);

            GameList.Remove(game);
        }

        /// <summary>
        /// Delete all games currently in the list
        /// </summary>
        public void DeleteAllGames()
        {
            lock (LockObj)
            {
                // remove game from websocket handling game
                lock (GameWebsocket.GameLock)
                {
                    GameWebsocket.Games.Clear();
                }

                GameList = new List<GameModel>();
            }
        }

        /// <summary>
        /// Join the specified game with the hash id
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public GameModel JoinGame(JoinGameMessage message)
        {

            if (string.IsNullOrWhiteSpace(message.GameHashId))
                throw HttpResponseExceptionHelper.Create("You must specify a game id to join",
                    HttpStatusCode.BadRequest);

            // Check if game exist
            lock (LockObj)
            {
                var game = GameList.FirstOrDefault(x => x.HashId == message.GameHashId);
                if (game == null)
                    throw HttpResponseExceptionHelper.Create("No game exist with specified hash to join",
                        HttpStatusCode.BadRequest);

                if (game.ParticipantsHashId.Contains(UserToken.UserId))
                    throw HttpResponseExceptionHelper.Create("You already are in this game",
                        HttpStatusCode.BadRequest);

                if (game.IsPrivate)
                {
                    if (string.IsNullOrWhiteSpace(message.Password))
                        throw HttpResponseExceptionHelper.Create("This game is private, tou need a password to join it",
                        HttpStatusCode.BadRequest);
                    if (game.Password != message.Password)
                        throw HttpResponseExceptionHelper.Create("Wrong password to join game",
                        HttpStatusCode.BadRequest);
                }

                if (game.MaxPlayersCount == game.CurrentPlayerCount)
                    throw HttpResponseExceptionHelper.Create("Game is full",
                        HttpStatusCode.BadRequest);

                if (game.State != EnumsModel.GameState.Waiting)
                    throw HttpResponseExceptionHelper.Create("Game has started or ended. You can't join it.",
                        HttpStatusCode.BadRequest);
                // OK
                game.CurrentPlayerCount++;
                game.ParticipantsHashId.Add(UserToken.UserId);

                return ToPublic(game);
            }
        }

        /// <summary>
        /// To become spectator
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public GameModel SpectateGame(JoinGameMessage message)
        {

            if (string.IsNullOrWhiteSpace(message.GameHashId))
                throw HttpResponseExceptionHelper.Create("You must specify a game id to spectate",
                    HttpStatusCode.BadRequest);

            // Check if game exist
            lock (LockObj)
            {
                var game = GameList.FirstOrDefault(x => x.HashId == message.GameHashId);
                if (game == null)
                    throw HttpResponseExceptionHelper.Create("No game exist with specified hash to spectate",
                        HttpStatusCode.BadRequest);

                game.SpectatorsHashId.Add(UserToken.UserId);

                return ToPublic(game);
            }
        }

        /// <summary>
        /// Reconnect (was in the disconnected list) to a game
        /// </summary>
        /// <param name="message"></param>
        public void ReconnectToGame(JoinGameMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.GameHashId))
                throw HttpResponseExceptionHelper.Create("You must specify a game id to reconnect",
                    HttpStatusCode.BadRequest);

            // Check if game exist
            lock (LockObj)
            {
                var game = GameList.FirstOrDefault(x => x.HashId == message.GameHashId);
                if (game == null)
                    throw HttpResponseExceptionHelper.Create("No game exist with specified hash to reconnect",
                        HttpStatusCode.BadRequest);

                if (!game.DisconnectedHashId.Contains(UserToken.UserId))
                    throw HttpResponseExceptionHelper.Create("You were not disconnected from this game",
                        HttpStatusCode.BadRequest);

                game.DisconnectedHashId.Remove(UserToken.UserId);
            }
        }

        /// <summary>
        /// Disconnect from game (use in test)
        /// </summary>
        /// <param name="message"></param>
        public void DisconnectOfGame(DisconnectMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.GameHashId))
                throw HttpResponseExceptionHelper.Create("You must specify a game id to disconnect",
                    HttpStatusCode.BadRequest);

            var game = GameList.FirstOrDefault(x => x.HashId == message.GameHashId);
            if (game == null)
                throw HttpResponseExceptionHelper.Create("No game exist with specified hash to disconnect",
                    HttpStatusCode.BadRequest);

            if (!game.ParticipantsHashId.Contains(UserToken.UserId))
                throw HttpResponseExceptionHelper.Create("You are not connected to this game",
                    HttpStatusCode.BadRequest);

            if (game.State == EnumsModel.GameState.Started)
            {
                game.DisconnectedHashId.Add(UserToken.UserId);
            }
        }

        /// <summary>
        /// Quit the game the user is in and delete it if the one who quit is the host
        /// </summary>
        /// <param name="gameHashId"></param>
        public void QuitGame(string gameHashId)
        {
            if (string.IsNullOrWhiteSpace(gameHashId))
                throw HttpResponseExceptionHelper.Create("You must specify a game id to quit",
                    HttpStatusCode.BadRequest);

            // Check if game exist
            lock (LockObj)
            {
                var game = GameList.FirstOrDefault(x => x.HashId == gameHashId);
                if (game == null)
                    throw HttpResponseExceptionHelper.Create("No game exist with specified hash to quit",
                        HttpStatusCode.BadRequest);

                if (game.HostHashId == UserToken.UserId)
                    DeleteGame(gameHashId);
                else
                {
                    if (game.ParticipantsHashId.Contains(UserToken.UserId))
                    {
                        if (game.State == EnumsModel.GameState.Started)
                        {
                            game.DisconnectedHashId.Add(UserToken.UserId);
                        }
                        else
                        {
                            game.ParticipantsHashId.Remove(UserToken.UserId);
                            game.CurrentPlayerCount--;
                        }
                    }
                    else if (game.SpectatorsHashId.Contains(UserToken.UserId))
                    {
                        game.SpectatorsHashId.Remove(UserToken.UserId);
                    }
                    else
                        throw HttpResponseExceptionHelper.Create("You are not in this game", HttpStatusCode.BadRequest);
                }
            }
        }

        /// <summary>
        /// Start the specified game
        /// </summary>
        /// <param name="gameHashId"></param>
        public void StartGame(string gameHashId)
        {
            if (string.IsNullOrWhiteSpace(gameHashId))
                throw HttpResponseExceptionHelper.Create("You must specify a game id to delete",
                    HttpStatusCode.BadRequest);

            // Check if game exist
            lock (LockObj)
            {
                var game = GameList.FirstOrDefault(x => x.HashId == gameHashId);
                if (game == null)
                    throw HttpResponseExceptionHelper.Create("No game exist with specified hash to delete",
                        HttpStatusCode.BadRequest);

                if (game.State != EnumsModel.GameState.Waiting)
                    throw HttpResponseExceptionHelper.Create("Game is already started or has finished",
                        HttpStatusCode.BadRequest);

                if (game.HostHashId != UserToken.UserId)
                    throw HttpResponseExceptionHelper.Create("Only host can start the game",
                        HttpStatusCode.BadRequest);

                // start the game and add it to the websocket handling the game
                game.State = EnumsModel.GameState.Started;
                lock (GameWebsocket.GameLock)
                {
                    GameWebsocket.Games.Add(game);
                }
            }
        }

    }
}