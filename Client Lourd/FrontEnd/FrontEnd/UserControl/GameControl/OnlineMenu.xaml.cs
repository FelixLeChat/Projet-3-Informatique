using System.Windows;
using FrontEnd.Core.Event;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for PlayMenu.xaml
    /// </summary>
    public partial class OnlineMenu
    {
        public OnlineMenu()
        {
            InitializeComponent();

            // Change Title
            MainWindow.Instance.SwitchTitle("Jeu en Ligne");
        }

        private void BtnOnlineGame_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.OnlineBoard });
        }

        private void BtnMatchMaking_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.MatchMaking });
        }

        private void BtnBackPlayMenu_OnClick(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.Back });
        }
    }
}
