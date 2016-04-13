using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FrontEnd.Game;
using FrontEnd.UserControl.CustomRoutedEvent;

namespace FrontEnd.UserControl.Partial
{
    /// <summary>
    /// Interaction logic for BasicOfflineGameConfig.xaml
    /// </summary>
    public partial class OnlineBasicGameConfigPanel
    {

        private BasicOnlineGameConfig _config;

        public OnlineBasicGameConfigPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Will set the datacontext (necessary for binding)
        /// </summary>
        /// <param name="offlineGameConfig"></param>
        public void SetGameConfig(BasicOnlineGameConfig offlineGameConfig)
        {
            _config = offlineGameConfig;
            DataContext = offlineGameConfig;
        }

        private void ZoneConfig_Checked(object sender, RoutedEventArgs e)
        {
            // Will bubble up the events to the parents
            var newEventArgs = new ZoneConfigChange(MyCustomEvent, _config.ZoneConfig);
            RaiseEvent(newEventArgs);
        }

        private void PlayerCountComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("2");
            data.Add("3");
            data.Add("4");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }

        // Todo: Check if can failed to update password
        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            _config.Password = passwordBox.Password;
        }

        // Todo: Check if can failed to update password
        private void StateChanged(object sender, RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        private void GameNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            _config.GameName = GameNameTxt.Text; 
        }
    }
}
