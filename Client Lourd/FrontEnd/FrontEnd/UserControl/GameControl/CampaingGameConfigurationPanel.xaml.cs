using System;
using System.Windows;
using FrontEnd.Core.Event;
using FrontEnd.Game.Config;
using FrontEnd.ProfileHelper;
using FrontEnd.Stats;
using FrontEnd.UserControl.Partial;
using Models;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for QuickGameConfiguration.xaml
    /// </summary>
    public partial class CampaingGameConfigurationPanel
    {
        private CampaingGameConfig _config;

        public CampaingGameConfigurationPanel()
        {
            InitializeComponent();

            // Change Title
            MainWindow.Instance.SwitchTitle("Configuration de la Campagne");
        }

        public void SetContext(CampaingGameConfig config)
        {
            _config = config;
            BasicGameConfig.SetGameConfig(config.Config);

            var mapSelector = new MultipleMapSelector();
            mapSelector.SetDataContext(config.MapConfig);
            ZoneSelectionPanel.Content = mapSelector;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() {Info = UiEvent.EventInfo.Back});
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_config.MapConfig.SelectedZones.Count <= 1)
            {
                MessageHelper.ShowMessage("Une long bal demande préparation", "Tu dois sélectionner au moins 2 danses (cartes)");
                return;
            }

            // Daily
            DailyManager.AchieveDaily(EnumsModel.DailyType.PlayCampain);

            if (_config == null)
            {
                throw new NullReferenceException("The config parameter should be set with SetContect() and therefor should not be null");
            }
            EventManager.Instance.Notice(new CreateGameRequestEvent() {Config = _config});
        }

    }
}
