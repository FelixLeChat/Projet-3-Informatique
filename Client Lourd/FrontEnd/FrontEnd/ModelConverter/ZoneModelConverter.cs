using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEnd.Game;
using FrontEnd.Model.ViewModel;

namespace FrontEnd.ModelConverter
{
    public static class ZoneModelConverter
    {
        public static ZoneViewModel ConvertZone(Zone zone)
        {
            var zoneVM = new ZoneViewModel()
            {
                HashId = zone.HashId,
                Name = zone.Name,
                Level = zone.Level,
                Path = zone.Path,
                ImagePath = zone.ImagePath
            };

            return zoneVM;
        }

        public static Zone ConvertZoneVM(ZoneViewModel zone)
        {
            var converted = new Zone()
            {
                HashId = zone.HashId,
                Name = zone.Name,
                Level = zone.Level,
                Path = zone.Path,
                ImagePath = zone.ImagePath
            };

            return converted;
        }
    }
}
