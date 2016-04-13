using System;
using FrontEnd.Game;
using FrontEndAccess.Ping;
using Models.Database;

namespace FrontEnd.ViewModel.Converter
{
    public static class MapModelConverter
    {
        public static Zone ConvertGameModel(MapModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var converted = new Zone
            {
                HashId = model.HashId,
                Name = model.Name,
                Level = model.Level,
                // TODO: get the real last update
                LastUpdateTimestamp = DateTime.Now.ToFileTimeUtc()
            };

            return converted;
        }
    }
}