using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using FrontEnd.Achievements;
using FrontEnd.Core;
using FrontEnd.Game;
using FrontEnd.Player;
using FrontEnd.Stats;
using FrontEnd.ViewModel.Converter;
using FrontEndAccess.APIAccess;
using Helper.Image;
using HttpHelper.Image;
using Models.Database;
using Newtonsoft.Json;

namespace FrontEnd.ProfileHelper
{
    /// <summary>
    /// Will wrap any logic with saving the zone
    ///   * Will create the .meta file necessary with the zone synchronization
    ///   * 
    /// </summary>
    public static class ZoneHelper
    {
        private const string DefaultImagePath = "/FrontEnd;component/Ressources/UI_Images/banner.png";

        public static string MetaExtension = "meta";
        public static string ImageExtension = "png";
        public static string XmlExtension = "xml";
        public static string OnlineRepo = "zones_online";
        public static string LocalRepo = "local";

        /// <summary>
        /// Get the name od the folder of a connected user
        /// </summary>
        /// <returns></returns>
        private static string GetUserZoneFolderName()
        {
            var folderName = string.Format("zones_{0}", User.Instance.Name);
            return folderName;
        }

        public static string GetParentZonePath()
        {
            StringBuilder path = new StringBuilder();
            path.Append(Path.Combine(Directory.GetCurrentDirectory()));
            path.Append(@"\zones\");
            return path.ToString();
        }

        /// <summary>
        /// Return the folder path where the player should check is zone (base if the player is online or not)
        /// </summary>
        /// <returns></returns>
        public static string GetZonePath(bool ifForOnlineGame = false)
        {
            StringBuilder path = new StringBuilder();

            if (ifForOnlineGame)
            {
                path.Append(Path.GetTempPath());
                path.Append(OnlineRepo);
            }
            else
            {
                path.Append(Path.Combine(Directory.GetCurrentDirectory()));
                path.Append(@"\zones\");

                if (User.Instance.IsConnected)
                {
                    path.Append(GetUserZoneFolderName());
                }
                else
                {
                    path.Append(LocalRepo);
                }

            }

            // Create directory if is doesn't exist
            Directory.CreateDirectory(path.ToString());

            return path.ToString();
        }

        /// <summary>
        /// Save the tree in the C++ 
        /// If the user is connected we need to save it on the server too
        /// </summary>
        /// <param name="path"></param>
        public static void Save(string path)
        {
            // Normal save
            NativeFunction.enregistrerFichierXML(path.ToCharArray(), path.Length);

            if (User.Instance.IsConnected)
            {
                var hash = SendOnServer(path);
                UpdateZoneMap(hash);

                // Stats, achievement and progress
                StatsManager.AddMapCreated();
                AchievementManager.AchieveFirstMapCreated();
                ProgressManager.TriggerProgress(ProgressManager.ProgressType.NewZone);
            }
            else
            {
                SaveOffline(path);

            }
        }

        private static void SaveOffline(string path)
        {
            var zoneName = Path.GetFileNameWithoutExtension(path);
            var zone = new Zone()
            {
                Name = zoneName,
                Path = path,
            };
            SaveMetaFile(zone, false);
        }

        private static string SendOnServer(string filePath)
        {
            var zoneName = Path.GetFileNameWithoutExtension(filePath);
            var content = File.ReadAllText(filePath);
            // TODO: Get level of map
            int level = NativeFunction.getNiveauCarte();

            var correspondingZone = GetZoneFromName(zoneName);
            if (correspondingZone != null)
            {
                UpdateZoneOnServer(correspondingZone.HashId, content, level);
            }
            else
            {
                var id = SaveOnServer(zoneName, content, level);

                correspondingZone = new Zone()
                {
                    HashId = id,
                    Name = zoneName,
                    Path = filePath,
                };
            }

            correspondingZone.Level = level;
            correspondingZone.LastUpdateTimestamp = DateTime.Now.ToFileTimeUtc();

            SaveMetaFile(correspondingZone, false);

            return correspondingZone.HashId;
        }



        /// <summary>
        /// Save the map on the server and the metafile
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="content"></param>
        /// <param name="level"></param>
        /// <param name="path"></param>
        private static string SaveOnServer(string zoneName, string content, int level)
        {
            try
            {
                string hash = ZoneAccess.Instance.CreateMap(zoneName, content, level);
                return hash;
            }
            catch (Exception e)
            {
                MessageHelper.ShowMessage("Erreur de sauvegarde", "Votre messager et son beau chapeau ont été incapable d'aller porter le schéma de votre bal dans la forteresse du Serveur");
                Console.WriteLine("Error to save map on server: \n{0}", e);
            }

            return null;
        }

        private static void UpdateZoneOnServer(string hashId, string content, int level)
        {
            try
            {
                ZoneAccess.Instance.UpdateMap(hashId, content, level);
            }
            catch (Exception e)
            {
                MessageHelper.ShowMessage("Erreur de mise à jour", "Votre messager et son beau chapeau ont été incapable d'aller porter le schéma de votre bal dans la forteresse du Serveur");
                Console.WriteLine("Error to update map on server: \n{0}", e);
            }
        }

        /// <summary>
        /// Work if the zone is load locally
        /// </summary>
        /// <param name="zoneName"></param>
        /// <returns></returns>
        private static Zone GetZoneFromName(string zoneName)
        {
            Zone correspondingZone = null;
            var zones = GetLocalZones();
            foreach (var zone in zones)
            {
                if (zone.Name == zoneName)
                {
                    correspondingZone = zone;
                    break;
                }
            }

            return correspondingZone;
        }

        private static void SaveMetaFile(Zone zone, bool isForOnlineGame)
        {
            var metaPath = GetMetaNameFromZone(zone, isForOnlineGame);

            //write string to file
            string json = JsonConvert.SerializeObject(zone);
            
            File.WriteAllText(metaPath, json);
        }

        public static string GetMetaNameFromZone(Zone zone, bool isForOnlineGame)
        {
            var metaPath = $@"{GetZonePath(isForOnlineGame)}\{zone.Name}.{MetaExtension}";
            return metaPath;
        }

        /// <summary>
        /// Save the image and return the path to this image
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string SaveImage(string id, string name, bool isForOnlineGame)
        {
            // Todo: change to use method
            var imgPath = $@"{GetZonePath(isForOnlineGame)}\{name}.{ImageExtension}";

            try
            {
                if (!File.Exists(imgPath))
                {
                    ZoneAccess.Instance.GetMapImage(id, imgPath);
                }
                else
                {
                    var zone = ZoneAccess.Instance.GetMapFromId(id);
                    var lastUpdateTime = zone.UpdateTime.Ticks;
                    var currentTime = File.GetLastWriteTimeUtc(imgPath).Ticks;
                    if (lastUpdateTime > currentTime)
                    {
                        ZoneAccess.Instance.GetMapImage(id, imgPath);
                    }
                }
            }
            catch (Exception)
            {
                imgPath = DefaultImagePath;
            }
            return imgPath;
        }

        public static string GetImagePathFromZone(Zone zone, bool isForOnlineGame)
        {
            var imgPath = $@"{GetZonePath(isForOnlineGame)}\{zone.Name}.{ImageExtension}";
            return imgPath;
        }

        private static void SaveImage(Zone zone, bool isForOnlineGame)
        {
            var imgPath = GetImagePathFromZone(zone, isForOnlineGame);

            try
            {
                if (!File.Exists(imgPath))
                {
                    zone.ImagePath = ZoneAccess.Instance.GetMapImage(zone.HashId, imgPath);
                }
                else
                {
                    zone.ImagePath = imgPath;
                }
            }
            catch (Exception)
            {
                zone.ImagePath = DefaultImagePath;
            }
        }

        public static void SaveDownloadedZone(Tuple<Zone, string> zoneContentTuple, bool isForOnlineGame = false)
        {
            var folderPath = GetZonePath(isForOnlineGame);
            var filePath = $@"{folderPath}\{zoneContentTuple.Item1.Name}.{XmlExtension}";

            File.WriteAllText(filePath, zoneContentTuple.Item2);
            zoneContentTuple.Item1.Path = filePath;
            //SaveImage(zoneContentTuple.Item1, isForOnlineGame);
            SaveMetaFile(zoneContentTuple.Item1, isForOnlineGame);     
        }

        public static List<Zone> GetMyLocalZones()
        {
            var zones = DoGetZones(GetZonePath());
            return zones;
        }

        /// <summary>
        /// Will search all available zone save on local
        /// </summary>
        /// <returns></returns>
        public static List<Zone> GetLocalZones()
        {
            var zones = DoGetZones(GetParentZonePath());
            zones = zones.Where(z => File.Exists(z.Path)).ToList();
            return zones;
        }

        private static List<Zone> DoGetZones(string folderPath)
        {
            var zones = new List<Zone>();

            var zoneMetaFiles = Directory.GetFiles(folderPath, $@"*.{MetaExtension}", SearchOption.AllDirectories);
            foreach (var zone in zoneMetaFiles)
            {
                using (StreamReader r = new StreamReader(zone))
                {
                    string json = r.ReadToEnd();
                    var item = JsonConvert.DeserializeObject<Zone>(json);
                    zones.Add(item);
                }
            }

            return zones;
        }

        public static List<Zone> GetAvailableZone()
        {
            if (User.Instance.IsConnected)
            {
                var list = GetAvailableMap().Where(m => m.Level <=  Profile.Instance.CurrentProfile.Level).ToList();
                var converted = list.ConvertAll(MapModelConverter.ConvertGameModel);
                //converted.ForEach(zone => SaveImage(zone, true));
                return converted;
            }
            else
            {
                return GetLocalZones();
            }
        }

        private static List<MapModel> GetAvailableMap()
        {
            var list = ZoneAccess.Instance.GetMyZones();
            list.AddRange(ZoneAccess.Instance.GetMyFriendsZones());
            list.AddRange(ZoneAccess.Instance.GetPublicZones());
            list = list.GroupBy(x => x.HashId).Select(y => y.First()).ToList();

            return list;
        }

        private static void UpdateZoneMap(string zoneHashId)
        {
            if (string.IsNullOrWhiteSpace(zoneHashId)) return;

            try
            {
                var path = Path.GetTempPath() + "default.png";
                ScreenShotHelper.SaveActiveMapHack(path);
                var img = Image.FromFile(path);
                var bArr = ImageHelper.ImgToByteArray(img);

                ZoneAccess.Instance.UpdateMapImage(zoneHashId, bArr);
            }
            catch { }
        }
    }
}