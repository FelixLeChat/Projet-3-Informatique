using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Game;
using FrontEnd.ProfileHelper;
using FrontEnd.ViewModel.Converter;
using FrontEndAccess.APIAccess;

namespace FrontEnd.AsyncLoading
{
    public static class ZoneSynchronizer
    {
        /// <summary>
        /// Will synchronize the user local zone with the online one
        /// </summary>
        public static void SynchronizeZone()
        {
            var zoneToDownload = new List<Zone>();
            var serverZones = GetServerZoneDict();
            var localZones = GetCurrentZoneDict();

            // If the user miss a zone or a more up to date one is one the server
            foreach (var serverZone in serverZones.Values)
            {
                Zone equivalentLocalZone;
                localZones.TryGetValue(serverZone.HashId, out equivalentLocalZone);
                if (equivalentLocalZone == null || equivalentLocalZone.LastUpdateTimestamp < serverZone.LastUpdateTimestamp)
                {
                    zoneToDownload.Add(serverZone);
                }
            }

            foreach (var localZone in localZones.Values)
            {
                // If a map is not on the server we delete it
                Zone equivalentServerZone;
                serverZones.TryGetValue(localZone.HashId, out equivalentServerZone);
                if (equivalentServerZone == null)
                {
                    DeleteZone(localZone);
                }
            }

            DownloadZone(zoneToDownload);
        }

        private static void DeleteZone(Zone zone)
        {
            File.Delete(zone.Path);
            File.Delete(ZoneHelper.GetMetaNameFromZone(zone, false));
            File.Delete(ZoneHelper.GetImagePathFromZone(zone, false));
        }

        /// <summary>
        /// Download the given zone id (if user is connected)
        /// </summary>
        /// <param name="zonesId"></param>
        public static void DownloadZone(List<Zone> zones, bool isForOnlineGame = false)
        {
            // Download in parallel
            //Parallel casse quand ont joue plusieurs fois la meme carte
            foreach (var zone in zones)
            {
                string content = ZoneAccess.Instance.GetMapFromId(zone.HashId).Content;
                ZoneHelper.SaveDownloadedZone(new Tuple<Zone, string>(zone, content), isForOnlineGame);
            }
        }

        public static List<Zone> DownloadZoneFromHashId(List<string> zonesId)
        {
            var zones = ConvertToMapModel(zonesId);
            DownloadZone(zones, true);
            return zones;
        }

        private static List<Zone> ConvertToMapModel(List<string> zonesId)
        {
            var zones = new List<Zone>();
            foreach (var id in zonesId)
            {
                var model = ZoneAccess.Instance.GetMapFromId(id);
                zones.Add(MapModelConverter.ConvertGameModel(model));
            }
            return zones;
        }

        /// <summary>
        /// Return a Dict[hashId: Zone] from his zone on the server
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Zone> GetServerZoneDict()
        {
            var zone = ZoneAccess.Instance.GetMyZones().ToDictionary(z => z.HashId, MapModelConverter.ConvertGameModel);
            return zone;
        }

        private static Dictionary<string, Zone> GetCurrentZoneDict()
        {
            var zone = ZoneHelper.GetMyLocalZones().ToDictionary(z => z.HashId, z => z);
            return zone;
        }
    }
}