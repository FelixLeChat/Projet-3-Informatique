using System;
using System.Windows;
using FrontEnd.Core.Event;
using FrontEnd.Game.Config;
using FrontEnd.ProfileHelper;
using FrontEnd.Stats;
using Models;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for QuickGameConfiguration.xaml
    /// </summary>
    public partial class QuickGameConfigurationPanel
    {
        private QuickGameConfig _config;

        public QuickGameConfigurationPanel()
        {
            InitializeComponent();

            // Change Title
            MainWindow.Instance.SwitchTitle("Configuration Partie Rapide");
        }

        public void SetContext(QuickGameConfig config)
        {
            _config = config;
            BasicGameConfig.SetGameConfig(config.BasicOfflineGameConfig);
            //Change name function and clas to zone instead of map
            ZoneSelector.SetDataContext(config.SingleMapConfig);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() {Info = UiEvent.EventInfo.Back});
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_config.SingleMapConfig.SelectedZone == null)
            {
                MessageHelper.ShowMessage("Patiente petite!", "Tu dois d'abord sélectionner une danse(carte)");
                return;
            }

            // Daily 
            DailyManager.AchieveDaily(EnumsModel.DailyType.PlayFastGame);

            if (_config == null)
            {
                throw new NullReferenceException("The config parameter should be set with SetContect() and therefor should not be null");
            }
            EventManager.Instance.Notice(new CreateGameRequestEvent() {Config = _config});
        }

        private void HandleChildEvent(object sender, RoutedEventArgs e)
        {
            if (_config == null)
            {
                throw new NullReferenceException("The config parameter should be set with SetContect() and therefor should not be null");
            }

            //if(_config.SingleMapConfig.SelectedZone != null)
            //    StartBtnDisabled.Visibility = Visibility.Collapsed;
        }
    }
}
