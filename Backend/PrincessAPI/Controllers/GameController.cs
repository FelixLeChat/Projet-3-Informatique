using System.Collections.Generic;
using System.Web.Http;
using Models.Communication;
using Models.Database;
using PrincessAPI.Controllers.SecureControllerHelper;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers
{
    [SecureAPI]
    [RoutePrefix("api/game")]
    public class GameController : SecureController
    {
        private readonly GameService _gameService;
        public GameController()
        {
            _gameService = new GameService(UserToken);
        }

        /// <summary>
        /// Get all current game 
        /// </summary>
        /// <returns>List of information on games</returns>
        [HttpGet]
        public List<GameModel> GetAllGames()
        {
            return _gameService.GetAllGames();
        }

        /// <summary>
        /// Obtain all the information on the given game 
        /// </summary>
        /// <param name="gameHashId">Id of the game</param>
        /// <returns>Information on the game</returns>
        [HttpGet]
        [Route("{gameHashId}")]
        public GameModel GetGameInfo(string gameHashId)
        {
            return _gameService.GetGameInfo(gameHashId);
        }

        /// <summary>
        /// Create a new game with the given information
        /// </summary>
        /// <param name="game">Information on the game</param>
        /// <returns>Id of the game</returns>
        [HttpPost]
        [Route("new")]
        public GameModel CreateGame(GameModel game)
        {
            return _gameService.CreateGame(game);
        }

        /// <summary>
        /// Join the specified game as a participant
        /// </summary>
        /// <param name="message">Details on the player joining</param>
        /// <returns>Information on the game</returns>
        [HttpPost]
        [Route("join")]
        public GameModel JoinGame(JoinGameMessage message)
        {
            return _gameService.JoinGame(message);
        }

        /// <summary>
        /// Join the specified game as a Spectator
        /// </summary>
        /// <param name="message">Details on the player joining</param>
        /// <returns>Information on the game</returns>
        [HttpPost]
        [Route("spectate")]
        public GameModel SpectateGame(JoinGameMessage message)
        {
            return _gameService.SpectateGame(message);
        }

        /// <summary>
        /// Reconnect to game when the player has disconnected
        /// </summary>
        /// <param name="message">Details on the player joining</param>
        [HttpPost]
        [Route("reconnect")]
        public void ReconnectToGame(JoinGameMessage message)
        {
            _gameService.ReconnectToGame(message);
        }

        /// <summary>
        /// Disconnect form the given game
        /// </summary>
        /// <param name="message">Information on the game to disconnect</param>
        [HttpPost]
        [Route("disconnect")]
        public void DisconnectFromGame(DisconnectMessage message)
        {
            _gameService.DisconnectOfGame(message);
        }

        /// <summary>
        /// Start the game as the host. You need to be the one who created the game
        /// </summary>
        /// <param name="gamehashId">Id of the game</param>
        [HttpPost]
        [Route("start/{gamehashId}")]
        public void StartGame(string gamehashId)
        {
            _gameService.StartGame(gamehashId);
        }

        /// <summary>
        /// Quit the specified game 
        /// </summary>
        /// <param name="gameHashId">Id of the game</param>
        [HttpPost]
        [Route("quit/{gameHashId}")]
        public void QuitGame(string gameHashId)
        {
            _gameService.QuitGame(gameHashId);
        }

        /// <summary>
        /// Delete the game that you created
        /// </summary>
        /// <param name="gameHashId">Id of the game</param>
        [HttpDelete]
        [Route("one/{gameHashId}")]
        public void DeleteGame(string gameHashId)
        {
            _gameService.DeleteGame(gameHashId);
        }

        /// <summary>
        /// Delete all the game
        /// </summary>
        [HttpDelete]
        [Route("all")]
        public void DeleteAllGames()
        {
            _gameService.DeleteAllGames();
        }
    }
}
