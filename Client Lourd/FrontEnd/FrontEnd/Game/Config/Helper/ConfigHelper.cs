using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FrontEnd.Game;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using Newtonsoft.Json;

namespace FrontEnd.Core.Helper
{
    /// <summary>
    /// Class that will proxy with the Native project with things related to config
    /// </summary>
    public static class ConfigHelper
    {
        public static string GetFolderPath()
        {
            var folderName = string.Format("config/config_{0}", User.Instance?.Name ?? "local");

            // Create directory if is doesn't exist
            Directory.CreateDirectory(folderName);

            return folderName;
        }

        private const string DefaultConfigPath = "DefaultCampain.json";
        private const string DefaultZonesgPath = "DefaultZones.json";

        /// <summary>
        /// Will retrieve default config for player count and player type
        /// </summary>
        /// <returns></returns>
        public static BasicOfflineGameConfig GetBasicConfig()
        {
            var config = new BasicOfflineGameConfig();

            // TODO: fix
            //config.PlayerCount = NativeFunction.obtenirNombreJoueur() ? PlayerCountMode.Solo : PlayerCountMode.Coop;
            //config.PlayerTypes = NativeFunction.obtenirEstHumain() ? PlayerType.Human : PlayerType.Computer;

            config.PlayerCount = PlayerCountMode.Solo;
            config.PlayerTypes = PlayerType.Human;

            return config;
        }

        public static BasicOfflineGameConfig GetCampaingDefaultSetting()
        {
            var path = GetFolderPath() + "/" + DefaultConfigPath;

            BasicOfflineGameConfig basicConfig = null;

            if (File.Exists(path))
            {
                try
                {
                    var serialize = File.ReadAllText(path);
                    basicConfig = JsonConvert.DeserializeObject<BasicOfflineGameConfig>(serialize);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while getting the default campain zone:\n{0}", e);
                }
                
            }
            if (basicConfig == null)
            {
                basicConfig = new BasicOfflineGameConfig();

                basicConfig.PlayerCount = PlayerCountMode.Solo;
                basicConfig.PlayerTypes = PlayerType.Human;
            }

            return basicConfig;
        }

        public static void SaveConfig(BasicOfflineGameConfig config)
        {
            var serialize = JsonConvert.SerializeObject(config);
            File.WriteAllText(GetFolderPath() + "/" + DefaultConfigPath, serialize);
        }

        /// <summary>
        /// Will retrieve all the available zones
        /// </summary>
        /// <returns></returns>
        public static List<Zone> GetAvailableZones()
        {
            //return ZoneHelper.GetLocalZones();
            return ZoneHelper.GetAvailableZone();
        }

        /// <summary>
        /// Will retrive the last campaing play
        /// </summary>
        /// <returns></returns>
        //public static List<Zone> GetDefaultCampaing()
        //{
        //    var zoneList = new List<Zone>();

        //    //Remplir le second tableau a partir des valeurs lues en c++ dans le fichier xml
        //    int nbFichiersXml = NativeFunction.obtenirNombreFichiers();
        //    StringBuilder sb = new StringBuilder(100);

        //    //String[] fichiersXml= new String[nbFichiersXml];
        //    for (int i = 0; i < nbFichiersXml; i++)
        //    {
        //        NativeFunction.obtenirFichier(sb, sb.Capacity, i);
        //        zoneList.Add(new Zone() { Name = sb.ToString().Substring(6) });
        //    }

        //    return zoneList;
        //}

        public static void SaveDefaultZone(List<Zone> zones)
        {
            var serialize = JsonConvert.SerializeObject(zones);
            File.WriteAllText(GetFolderPath() + "/" + DefaultZonesgPath, serialize);
        }

        public static List<Zone> GetDefaultCampaingZone()
        {
            List<Zone> list = null;

            var path = GetFolderPath() + "/" + DefaultZonesgPath;

            if (File.Exists(path))
            {
                try
                {
                    var serialize = File.ReadAllText(path);
                    if (!string.IsNullOrEmpty(serialize))
                    {
                        list = JsonConvert.DeserializeObject<List<Zone>>(serialize);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while getting the default campain zone:\n{0}", e);
                }
            }
            if (list == null)
            {
                list = new List<Zone>();
            }
            return list;
        }

        /// <summary>
        /// From the name of the zone, will return the relative path for the C++
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static StringBuilder ConvertZoneName(string name)
        {
            StringBuilder convert = new StringBuilder("zones\\");
            convert.Append(name);
            return convert;
        }

        /// <summary>
        /// From the name of the zone, will return the relative path for the C++
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static char[] ConvertZoneNameToArray(string name)
        {
            var tab = (ConvertZoneName(name).ToString()).ToCharArray();
            return tab;
        }



        /// <summary>
        /// Converter from the C# representation of player count to the C++ representation (PlayerCount)
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int ConvertPlayerCount(PlayerCountMode count)
        {
            int convertValue = (int)count;
            return convertValue;
        }

        /// <summary>
        /// Converter from the C# representation of player count to the C++ representation (isSolo)
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ConvertIsSolo(PlayerCountMode count)
        {
            bool convertValue = count == PlayerCountMode.Solo;
            return convertValue;
        }

        /// <summary>
        /// Converter from the C# representation of player type to the C++ representation (isHuman)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ConvertPlayerType(PlayerType type)
        {
            bool convertValue = type == PlayerType.Human;
            return convertValue;
        }

    }
}