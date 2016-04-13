using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEndAccess.APIAccess;
using Models;
using Models.Database;

namespace FrontEnd.Helper
{
    /// <summary>
    /// Check many property to check if player can join the game, and display explaining message if not
    /// </summary>
    public static class JoinGameHelper
    {
        /// <summary>
        /// Look if the player is already in the game but was disconnected some-how
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static bool IsReconnection(string gameId)
        {
            try
            {
                var game = GameAccess.Instance.GetGameInfo(gameId);
                var playerId = Profile.Instance.CurrentProfile.UserHashId;

                return game.DisconnectedHashId.Contains(playerId);
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
            /// Use for matchmaking
            /// </summary>
            /// <returns></returns>
        public static List<GameModel> GetAvailableGameForPlayers()
        {
            var possibleGames = GameAccess.Instance.GetAllGames().Where(g => CanJoin(g.HashId, false) && !g.IsPrivate).ToList();
            return possibleGames;
        }

        /// <summary>
        /// Check condition without trying to join
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="showExplainingMessage"></param>
        /// <returns></returns>
        public static bool CanJoin(string hashId, bool showExplainingMessage)
        {
            GameModel model = null;
            try
            {
                model = GameAccess.Instance.GetGameInfo(hashId);
            }
            catch (Exception e)
            {
                if (showExplainingMessage)
                {
                    MessageHelper.ShowError("Ce n'est pas le bal que vous cherché", "Ce bal n'a jamais existé ou n'existe plus", e);
                }
                return false;
            }

            var canJoin = GameIsNotStarted(model, showExplainingMessage) && PlaceAvailable(model, showExplainingMessage) && CheckLevelRestriction(model, showExplainingMessage);
            return canJoin;
        }


        private static bool CheckLevelRestriction(GameModel model, bool showExplainingMessage)
        {
            var canJoin = model.Level <= Profile.Instance.CurrentProfile.Level;

            if (!canJoin && showExplainingMessage)
            {
                MessageHelper.ShowMessage("Vous n'êtes pas encore assez princesse!", 
                    "Ce bal est tellement sophistiqué que vous devez accèder à un titre plus élevé");
            }

            return canJoin;
        }

        private static bool PlaceAvailable(GameModel model, bool showExplainingMessage)
        {
            var canJoin = model.CurrentPlayerCount < model.MaxPlayersCount;

            if (!canJoin && showExplainingMessage)
            {
                MessageHelper.ShowMessage("Ce bal est complet!", 
                    "Merci de votre intérêt princesse, mais votre prince charmant se trouve dans un autre château!");
            }

            return canJoin;
        }

        private static bool GameIsNotStarted(GameModel model, bool showExplainingMessage)
        {
            var canJoin = model.State == EnumsModel.GameState.Waiting;

            if (!canJoin && showExplainingMessage)
            {
                switch (model.State)
                {
                    case EnumsModel.GameState.Started:
                        MessageHelper.ShowMessage("Les portes du bal sont fermés!", "Il est impoli de joindre un bal déjà commencé");
                        break;
                    case EnumsModel.GameState.Ended:
                        MessageHelper.ShowMessage("Le bal est fini!", "Tout le monde est déjà soul");
                        break;
                }
            }

            return canJoin;
        }
    }
}