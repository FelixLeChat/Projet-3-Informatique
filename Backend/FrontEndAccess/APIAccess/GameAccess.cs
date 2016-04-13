using System;
using System.Collections.Generic;
using System.Net;
using Helper.Http;
using Models.Communication;
using Models.Database;
using Newtonsoft.Json;

namespace FrontEndAccess.APIAccess
{
    public class GameAccess
    {
        public static GameAccess Instance;
        public string Endpoint;

        // Set endpoint to spedified value
        public GameAccess(string endpoint)
        {
            Instance = this;
            Endpoint = endpoint + "/api/game";
        }

        /// <summary>
        /// Get all game that are currently on the server 
        /// </summary>
        /// <returns></returns>
        public List<GameModel> GetAllGames()
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<List<GameModel>>(body);
        }

        /// <summary>
        /// Get info one one specific game
        /// </summary>
        /// <param name="gameHashId"></param>
        /// <returns></returns>
        public GameModel GetGameInfo(string gameHashId)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint+"/"+gameHashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<GameModel>(body);
        }

        /// <summary>
        /// Create a new game with the specified information
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public GameModel CreateGame(GameModel game)
        {
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/new", game, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<GameModel>(body);
        }
        public GameModel CreateGame(string name, int maxPlayer, List<string> zonesHash, string password="")
        {
            var isPrivate = !string.IsNullOrWhiteSpace(password);
            var gameModel = new GameModel()
            {
                Name = name,
                MaxPlayersCount = maxPlayer,
                IsPrivate = isPrivate,
                Password = password,
                ZonesHashId = zonesHash
            };

            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/new", gameModel, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<GameModel>(body);
        }

        /// <summary>
        /// Delete the game identified by the given hash id
        /// </summary>
        /// <param name="gameHashId"></param>
        public void DeleteGame(string gameHashId)
        {
            if (string.IsNullOrWhiteSpace(gameHashId)) return;

            var httpResponse = HttpRequestHelper.DeleteAsync(Endpoint + "/one/" + gameHashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        /// <summary>
        /// Join the game with the specified hash id
        /// </summary>
        /// <param name="gameHashId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public GameModel JoinGame(string gameHashId, string password = "")
        {
            var message = new JoinGameMessage() {GameHashId = gameHashId, Password = password};
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/join", message, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<GameModel>(body);
        }

        /// <summary>
        /// To spectate a game
        /// </summary>
        /// <param name="gameHashId"></param>
        /// <returns></returns>
        public GameModel Spectate(string gameHashId)
        {
            var message = new JoinGameMessage() { GameHashId = gameHashId};
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/spectate", message, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<GameModel>(body);
        }

        /// <summary>
        /// Call when a player present in the disconnect list want to join back the game
        /// </summary>
        /// <param name="gameHashId"></param>
        public void ReconnectGame(string gameHashId)
        {
            var message = new JoinGameMessage() { GameHashId = gameHashId};
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/reconnect", message, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        /// <summary>
        /// Disconnect from game
        /// </summary>
        /// <param name="gameHashId"></param>
        public void DisconnectGame(string gameHashId)
        {
            var message = new JoinGameMessage() { GameHashId = gameHashId };
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/disconnect", message, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        /// <summary>
        /// Quit the game if you are currently in it. If you are the host, the game is deleted
        /// </summary>
        /// <param name="gameHashId"></param>
        public void QuitGame(string gameHashId)
        {
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/quit/"+gameHashId, null, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        /// <summary>
        /// The host can start the game he host given the right gameHashId
        /// </summary>
        /// <param name="gameHashId"></param>
        public void StartGame(string gameHashId)
        {
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/start/" + gameHashId, null, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }
    }
}
