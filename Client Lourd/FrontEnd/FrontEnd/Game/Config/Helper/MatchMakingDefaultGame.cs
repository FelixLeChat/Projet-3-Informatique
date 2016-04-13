using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEndAccess.APIAccess;
using Models.Database;

namespace FrontEnd.Game.Config.Helper
{
    public static class MatchMakingDefaultGame
    {
        private static Random _random = new Random();

        public static GameModel GetModelForLevel(int level)
        {
            var game = new GameModel()
            {
                IsPrivate = false,
                IsCoop = false,
                MaxPlayersCount = 2,
                Level = level,
                Name = $"Matchmaking #{_random.Next(int.MaxValue)}",
                ZonesHashId = new List<string>() { GameMapRespectingLevel(level) }
            };
            return game;
        }

        private static string GameMapRespectingLevel(int level)
        {
            var list = ZoneAccess.Instance.GetPublicZones().Where(z => z.Level <= level).ToList();
            if (list.Any())
            {
                int r = _random.Next(list.Count);
                return list[r].HashId;
            }
            return "";
        }


    }
}
