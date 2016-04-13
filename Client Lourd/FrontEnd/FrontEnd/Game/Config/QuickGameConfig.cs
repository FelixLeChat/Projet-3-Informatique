using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FrontEnd.AsyncLoading;
using FrontEnd.Core;
using FrontEnd.Core.Helper;
using FrontEnd.Game.Config.Common;
using FrontEnd.Game.Config.Helper;
using FrontEnd.Game.Wrap;
using FrontEnd.Model.ViewModel;
using FrontEnd.ModelConverter;
using FrontEnd.ProfileHelper;
using FrontEnd.UserControl;
using FrontEnd.UserControl.Winform;

namespace FrontEnd.Game.Config
{
    public class QuickGameConfig : IGameConfig
    {
        public BasicOfflineGameConfig BasicOfflineGameConfig { get; set; }
        public SingleMapConfig SingleMapConfig { get; set; }

        /// <summary>
        /// Init the config for the quick game (2ep player options, and available zones)
        /// </summary>
        public void InitConfig()
        {
            BasicOfflineGameConfig = ConfigHelper.GetBasicConfig();
            var availableZones = ConfigHelper.GetAvailableZones();
            var zoneViewModel = availableZones.ConvertAll(ZoneModelConverter.ConvertZone);
            ZoneViewModel.LoadImagesAsync(zoneViewModel);
            SingleMapConfig = new SingleMapConfig(zoneViewModel);
        }

        /// <summary>
        /// Switch to the quickGameConfiguration
        /// </summary>
        public void GotoConfigWindows()
        {
            var quickGameConfiguration = new QuickGameConfigurationPanel();
            quickGameConfiguration.SetContext(this);
            Program.MainWindow.SwitchScreen(quickGameConfiguration);
        }

        public bool Validate()
        {
            throw new System.NotImplementedException();
        }

        public IGame CreateGame()
        {
            var game = new LocalGame(IntegratedOpenGl.Mode.ModePartieRapide) { Config = this };
            return game;
        }

        public void PreSetup()
        {
            ZoneViewModel zone = SingleMapConfig.SelectedZone;
            if (!File.Exists(zone.Path))
            {
                var result = ZoneSynchronizer.DownloadZoneFromHashId(new List<string> { zone.HashId });
                SingleMapConfig.SelectedZone = ZoneModelConverter.ConvertZone(result[0]);

                // Hack for achievement
                GameAchievementHelper.CheckForAchievementTask(result);
            }
        }

        public void Setup()
        {
            ZoneViewModel zone = SingleMapConfig.SelectedZone;
            NativeFunction.ouvrirPartieRapide(ConfigHelper.ConvertPlayerCount(BasicOfflineGameConfig.PlayerCount), ConfigHelper.ConvertPlayerType(BasicOfflineGameConfig.PlayerTypes), zone.Path.ToCharArray(), zone.Path.Length);
        }
    }
}