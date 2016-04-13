using System.Windows;
using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for PlayMenu.xaml
    /// </summary>
    public partial class GameMenu
    {
        public GameMenu()
        {
            InitializeComponent();

            if (User.Instance.IsConnected)
            {
                BtnPlayOnlineGray.Visibility = Visibility.Collapsed;
            }

            // Change Title
            MainWindow.Instance.SwitchTitle("Jeu");
        }

        private void BtnPlayOffline_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.OfflineGame });
        }
        private void BtnPlayOnline_Click(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.OnlineGame });
        }
        private void BtnBackMainMenu_OnClick(object sender, RoutedEventArgs e)
        {
            RequestChangeState(Enums.States.MainMenu);
        }
        private void RequestChangeState(Enums.States state)
        {
            EventManager.Instance.Notice(new ChangeStateEvent() { NextState = state });
        }


        private void BtnPlayOnlineGray_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageHelper.ShowMessage("Vous ne pouvez pas faire ca", "Vous avez besoin d'un messager avec un beau chapeau nommé Mr.Internet");
        }
    }
}
