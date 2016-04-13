using System.Collections.Generic;
using System.Linq;

namespace Models.Database
{
    public class GameModel
    {
        public string HashId { get; set; }
        public string Name { get; set; }
        public EnumsModel.GameState State { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsCoop { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }

        // List of unique Hash of users
        public List<string> ParticipantsHashId { get; set; } = new List<string>();
        public List<string> DisconnectedHashId { get; set; } = new List<string>();
        public string HostHashId { get; set; }
        public List<string> ZonesHashId { get; set; }  = new List<string>();
        public List<string> SpectatorsHashId { get; set; } = new List<string>();
        public int MaxPlayersCount { get; set; }
        public int CurrentPlayerCount { get; set; }
    }

    public class GameModelHelper
    {
        private const int MinPlayerInGame = 1;
        private const int MaxPlayerInGame = 5;

        public static GameModel ToPublic(GameModel game)
        {
            return new GameModel()
            {
                HashId = game.HashId,
                Name = game.Name,
                State = game.State,
                IsPrivate = game.IsPrivate,
                MaxPlayersCount = game.MaxPlayersCount,
                CurrentPlayerCount = game.CurrentPlayerCount,
                ParticipantsHashId = game.ParticipantsHashId,
                IsCoop = game.IsCoop,
                HostHashId = game.HostHashId,
                ZonesHashId = game.ZonesHashId,
                SpectatorsHashId = game.SpectatorsHashId,
                Level = game.Level,
                DisconnectedHashId = game.DisconnectedHashId
            };
        }

        public static List<GameModel> ToPublic(List<GameModel> game)
        {
            return game.Select(ToPublic).ToList();
        }

        public static bool IsValid(GameModel game)
        {
            if (string.IsNullOrWhiteSpace(game.Name))
                return false;
            if (game.MaxPlayersCount < MinPlayerInGame || game.MaxPlayersCount > MaxPlayerInGame)
                return false;
            if (game.IsPrivate && string.IsNullOrWhiteSpace(game.Password))
                return false;
            if (game.ZonesHashId == null || game.ZonesHashId.Count == 0)
                return false;
            return true;
        }
    }
}