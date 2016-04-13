using System.Windows;
using FrontEnd.Core.Event;
using FrontEnd.Game;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl.GameControl
{
    /// <summary>
    /// Interaction logic for WaitingRoom.xaml
    /// </summary>
    public partial class WaitingRoom
    {
        public WaitingRoom()
        {
            InitializeComponent();
            SetContext(OnlineSession.Instance);

            // Change Title
            MainWindow.Instance.SwitchTitle("Salle d'attente");
        }

        public void SetContext(OnlineSession session)
        {
            DataContext = session;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            EventManager.Instance.Notice(new UiEvent() { Info = UiEvent.EventInfo.Back });
        }


    }
}
