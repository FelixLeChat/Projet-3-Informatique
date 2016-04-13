using System;
using System.Collections.Generic;
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
using FrontEnd.UserControl;
using FrontEnd.UserControl.Winform;

namespace FrontEnd.Game.Config
{
    public class CampaingGameConfig: IGameConfig
    {
        // Todo: change to private
        public BasicOfflineGameConfig Config { get; set; }
        public MultiMapConfig MapConfig { get; set; }

        public void Initialize()
        {
            // get default campaning xone
        }

        public void InitConfig()
        {
            Config = ConfigHelper.GetCampaingDefaultSetting();
            var availableZones = ConfigHelper.GetAvailableZones();
            var selectedZones = ConfigHelper.GetDefaultCampaingZone();

            var zoneViewModel = availableZones.ConvertAll(ZoneModelConverter.ConvertZone);
            ZoneViewModel.LoadImagesAsync(zoneViewModel);

            var selectedModel = selectedZones.ConvertAll(ZoneModelConverter.ConvertZone);
            ZoneViewModel.LoadImagesAsync(selectedModel);

            MapConfig = new MultiMapConfig(zoneViewModel, selectedModel);
        }

        public void GotoConfigWindows()
        {
            var quickGameConfiguration = new CampaingGameConfigurationPanel();
            quickGameConfiguration.SetContext(this);
            Program.MainWindow.SwitchScreen(quickGameConfiguration);
        }

        public bool Validate()
        {
            return MapConfig.SelectedZones.Count > 1;
        }

        public IGame CreateGame()
        {
            ConfigHelper.SaveConfig(Config);
            ConfigHelper.SaveDefaultZone(ObservableCollectionConverter.ConvertObservableCollection(MapConfig.SelectedZones).ConvertAll(ZoneModelConverter.ConvertZoneVM));

            var game = new LocalGame(IntegratedOpenGl.Mode.ModeCampagne) { Config = this };
            return game;
        }

        public void PreSetup()
        {
            //On envoie la liste de xml a c++
            for (int index = 0; index < MapConfig.SelectedZones.Count; index++)
            {
                var zone = MapConfig.SelectedZones[index];
                if (!File.Exists(zone.Path))
                {
                    // The file is not downloaded
                    var result = ZoneSynchronizer.DownloadZoneFromHashId(new List<string> {zone.HashId});
                    MapConfig.SelectedZones[index] = ZoneModelConverter.ConvertZone(result[0]);

                    // Hack for achievement
                    GameAchievementHelper.CheckForAchievementTask(result);
                }
            }
        }

        public void Setup()
        {
            //Todo: what is this path????
            StringBuilder c = new StringBuilder("données\\campagneDefaut.xml");
            NativeFunction.ouvrirPartieCampagne(ConfigHelper.ConvertPlayerCount(Config.PlayerCount), ConfigHelper.ConvertPlayerType(Config.PlayerTypes), c);

            NativeFunction.transmettreJoueursHumain(ConfigHelper.ConvertIsSolo(Config.PlayerCount), ConfigHelper.ConvertPlayerType(Config.PlayerTypes));
            
            //On envoie la liste de xml a c++
            foreach (var zone in MapConfig.SelectedZones)
            {
                if (!NativeFunction.envoyerNomFichier(zone.Path.ToCharArray(), zone.Path.Length))
                {
                    //Raise alert
                }
            }

            NativeFunction.finReceptionNomFichiers();
        }
    }
}