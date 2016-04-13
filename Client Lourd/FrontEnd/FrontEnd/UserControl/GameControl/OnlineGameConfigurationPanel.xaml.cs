using System;
using System.Windows;
using FrontEnd.Core.Event;
using FrontEnd.Game;
using FrontEnd.Game.Config;
using FrontEnd.UserControl.CustomRoutedEvent;
using FrontEnd.UserControl.Partial;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl.GameControl
{
    /// <summary>
    /// Interaction logic for QuickGameConfiguration.xaml
    /// </summary>
    public partial class OnlineGameConfigurationPanel
    {
        private OnlineGameConfig _config;
        private readonly SingleMapSelector _singleZoneSelector;
        private readonly MultipleMapSelector _multipleMapSelector;

        public OnlineGameConfigurationPanel()
        {
            InitializeComponent();
            _singleZoneSelector = new SingleMapSelector();
            _multipleMapSelector = new MultipleMapSelector();

            // Change Title
            MainWindow.Instance.SwitchTitle("Configuration de la Partie");
        }

        public void SetContext(OnlineGameConfig config)
        {
            _config = config;
            BasicGameConfig.SetGameConfig(config.GameConfig);

            _singleZoneSelector.SetDataContext(config.SingleMapConfig);
            _multipleMapSelector.SetDataContext(config.MultipleMapConfig);
            _singleZoneSelector.ChildEvent += HandleChildEvent;
            _multipleMapSelector.ChildEvent += HandleChildEvent;

            SwitchZoneSelector(_config.GameConfig.ZoneConfig);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.Back });
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_config == null)
            {
                throw new NullReferenceException("The config parameter should be set with SetContect() and therefor should not be null");
            }

            if (!_config.Validate())
            {
                return;
            }

            EventManager.Instance.Notice(new CreateGameRequestEvent() { Config = _config });
        }

        private void HandleChildEvent(object sender, RoutedEventArgs e)
        {
            if (_config == null)
            {
                throw new NullReferenceException("The config parameter should be set with SetContect() and therefor should not be null");
            }

            var zoneConfigChange = e as ZoneConfigChange;
            if (zoneConfigChange != null)
            {
                SwitchZoneSelector(zoneConfigChange.ZoneConfig);
            }
        }

        private void SwitchZoneSelector(ZoneConfig zoneConfig)
        {
            switch (zoneConfig)
            {
                case ZoneConfig.SoloMap:
                    ZoneSelectionPanel.Content = _singleZoneSelector;
                    break;
                case ZoneConfig.MultiMap:
                    ZoneSelectionPanel.Content = _multipleMapSelector;
                    break;
            }
        }
    }
}
