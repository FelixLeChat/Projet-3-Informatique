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
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for PlayMenu.xaml
    /// </summary>
    public partial class OfflineMenu
    {

        public OfflineMenu()
        {
            InitializeComponent();

            // Change Title
            MainWindow.Instance.SwitchTitle("Jeu Hors Ligne");
        }

        private void BtnQuickGame_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.QuickGame });
        }

        private void BtnCampaing_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.Campaign });
        }

        private void BtnBackPlayMenu_OnClick(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.Back });
        }

    }
}
