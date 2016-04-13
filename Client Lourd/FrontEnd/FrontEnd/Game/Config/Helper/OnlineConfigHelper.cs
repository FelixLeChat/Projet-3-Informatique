using System;
using System.Collections.Generic;
using System.Text;
using FrontEnd.Core.Helper;
using FrontEnd.Game.Wrap;
using Models.Database;

namespace FrontEnd.Game.Config.Helper
{
    public class OnlineConfigHelper
    {
        public bool IsCoop { get; set; }
        public int PlayerCount { get; set; }
        public int IaCount { get; set; }
        public char[] MatchId { get; set; }
        public int MathIdLength { get; set; }
        public String[] PlayersId { get; set; }
        public int[] PlayersIdLength { get; set; }
        public int ZoneCount { get; set; }
        public String[] ZonesPath { get; set; }
        public int[] ZonesPathLength { get; set; }
        public bool IsHost { get; set; }

        public OnlineConfigHelper(GameModel game, List<Zone> zones)
        {
            // Todo; add coop info in model
            IsCoop = game.IsCoop;
            PlayerCount = game.CurrentPlayerCount;
            IaCount = game.MaxPlayersCount - game.CurrentPlayerCount;
            MatchId = game.HashId.ToCharArray();
            MathIdLength = game.HashId.Length;
            var players = new List<String>();
            var playersLength = new List<int>();
            foreach (var player in game.ParticipantsHashId)
            {
                players.Add(player);
                playersLength.Add(player.Length);
            }
            PlayersId = players.ToArray();
            PlayersIdLength = playersLength.ToArray();

            var paths = new List<String>();
            var pathsLength = new List<int>();
            ZoneCount = game.ZonesHashId.Count;

            foreach (var zone in zones)
            {
                paths.Add(zone.Path);
                pathsLength.Add(zone.Path.Length);
            }

            ZonesPath = paths.ToArray();
            ZonesPathLength = pathsLength.ToArray();

        }
    }
}