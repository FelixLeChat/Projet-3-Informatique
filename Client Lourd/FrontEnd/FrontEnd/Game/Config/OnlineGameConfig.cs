using System;
using System.Collections.Generic;
using System.Linq;
using FrontEnd.Core;
using FrontEnd.Core.Helper;
using FrontEnd.Game.Config.Common;
using FrontEnd.Game.Wrap;
using FrontEnd.Model.ViewModel;
using FrontEnd.ModelConverter;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.UserControl.GameControl;
using FrontEnd.UserControl.Winform;
using Models.Database;

namespace FrontEnd.Game.Config
{
    public class OnlineGameConfig : IGameConfig
    {
        public BasicOnlineGameConfig GameConfig { get; set; }
        public SingleMapConfig SingleMapConfig { get; set; }
        public MultiMapConfig MultipleMapConfig { get; set; }

        /// <summary>
        /// Init the config for the quick game (2ep player options, and available zones)
        /// </summary>
        public void InitConfig()
        {
            GameConfig = new BasicOnlineGameConfig();
            var availableZones = ConfigHelper.GetAvailableZones();
            var zoneViewModel = availableZones.ConvertAll(ZoneModelConverter.ConvertZone);
            ZoneViewModel.LoadImagesAsync(zoneViewModel);
            SingleMapConfig = new SingleMapConfig(zoneViewModel);
            MultipleMapConfig = new MultiMapConfig(zoneViewModel, new List<ZoneViewModel>());
        }

        /// <summary>
        /// Switch to the quickGameConfiguration
        /// </summary>
        public void GotoConfigWindows()
        {
            var configurationWindows = new OnlineGameConfigurationPanel();
            configurationWindows.SetContext(this);
            Program.MainWindow.SwitchScreen(configurationWindows);
        }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(GameConfig.GameName))
            {
                MessageHelper.ShowMessage("Woah minute!!!", "Un bal mémorable à besoin d'un nom approprié");
                return false;
            }
            if (GameConfig.IsPrivate && string.IsNullOrWhiteSpace(GameConfig.Password))
            {
                MessageHelper.ShowMessage("Woah minute!!!", "Un bal privé, demande un mot de passe pour filtré les non-désirables");
                return false;
            }
            if (GameConfig.ZoneConfig == ZoneConfig.SoloMap && !SingleMapConfig.Validate())
            {
                return false;
            }
            if (GameConfig.ZoneConfig == ZoneConfig.MultiMap && !MultipleMapConfig.Validate())
            {
                return false;
            }

            return true;
        }

        public IGame CreateGame()
        {
            if (!Validate())
            {
                throw new Exception("The game option are not valid");
            }
            var game = new OnlineGame(GetOpenGlMode(), GetGameModel());

            return game;
        }

        public void PreSetup()
        {

        }

        public GameModel GetGameModel()
        {
            var currentPlayer = Profile.Instance.CurrentProfile;

            // Todo: get highest zone level
            var model = new GameModel()
            {
                Name = GameConfig.GameName,
                IsPrivate = GameConfig.IsPrivate,
                Password = GameConfig.IsPrivate ? GameConfig.Password : "",
                MaxPlayersCount = GameConfig.PlayerCount,
                ZonesHashId = GetZonesHashIdList(),
                HostHashId = currentPlayer.UserHashId,
                Level = GetGameLevel(),
                IsCoop =  GameConfig.MultiMode == MultiMode.Coop
            };

            return model;
        }

        private List<string> GetZonesHashIdList()
        {
            if (GameConfig.ZoneConfig == ZoneConfig.SoloMap)
            {
                return new List<string>() { SingleMapConfig.SelectedZone.HashId };
            }
            else
            {
                return MultipleMapConfig.SelectedZones.Select(x => x.HashId).ToList();
            }
        }

        private int GetGameLevel()
        {
            List<ZoneViewModel> zones;
            if (GameConfig.ZoneConfig == ZoneConfig.SoloMap)
            {
                zones = new List<ZoneViewModel>() {SingleMapConfig.SelectedZone};
            }
            else
            {
                zones = MultipleMapConfig.SelectedZones.ToList();
            }

            return zones.Max(x => x.Level);
        }

        /// <summary>
        /// Convert to mode that C++ understrand
        /// </summary>
        /// <returns></returns>
        private IntegratedOpenGl.Mode GetOpenGlMode()
        {
            return GameConfig.ZoneConfig == ZoneConfig.SoloMap ? IntegratedOpenGl.Mode.ModePartieRapide : IntegratedOpenGl.Mode.ModeCampagne;
        }

        public void Setup()
        {
            // This config is only use by the host, the setup method has been moved to OnlineGame
        }
    }
}