using System;
using Models.Database;
using FrontEndAccess.Ping;

namespace FrontEnd.ViewModel.Converter
{
    public static class GameModelConverter
    {
        public static GameViewModel ConvertGameModel(GameModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var converted = new GameViewModel
            {
                Id = model.HashId,
                Name = model.Name,
                IsPrivate = model.IsPrivate,
                State = model.State,
                Population = $"{model.CurrentPlayerCount}/{model.MaxPlayersCount}",
                Level = model.Level,
                // Todo: make ping relevant to the target game (lol)
                Ping = PingAccess.Instance.GetPingMs()
            };

            return converted;
        }
    }
}