using System.Windows;
using FrontEnd.Game;

namespace FrontEnd.UserControl.Partial
{
    /// <summary>
    /// Interaction logic for BasicOfflineGameConfig.xaml
    /// </summary>
    public partial class OfflineBasicGameConfigPanel
    {

        private BasicOfflineGameConfig _config;

        public OfflineBasicGameConfigPanel()
        {
            InitializeComponent();
        }

        public void SetGameConfig(BasicOfflineGameConfig offlineGameConfig)
        {
            _config = offlineGameConfig;
            this.DataContext = offlineGameConfig;
        }

        private void PlayerCount_Checked(object sender, RoutedEventArgs e)
        {
            PlayerTypeGroupBox.IsEnabled = _config.PlayerCount == PlayerCountMode.Coop;
        }
    }
}
