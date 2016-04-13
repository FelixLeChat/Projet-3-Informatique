using System.Linq;
using FrontEnd.Player;
using FrontEndAccess.APIAccess;
using Models.Database;

namespace FrontEnd.Helper
{
    /// <summary>
    /// Utility functions to detect if a player can join a game still available where he disconnect from
    /// </summary>
    public static class ReconnectHelper
    {
        public static bool CanJoinBackGame(out GameModel foundGame)
        {
            var games = GameAccess.Instance.GetAllGames();
            foundGame = games.FirstOrDefault(game => game.DisconnectedHashId.Contains(Profile.Instance.CurrentProfile.UserHashId));
            return foundGame != null;
        }

    }
}