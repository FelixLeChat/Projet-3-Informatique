using System.Windows;
using FrontEnd.AsyncLoading;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEndAccess;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl.Partial
{
    public partial class LoginHeader
    {
        private readonly User _user;

        public LoginHeader()
        {
            InitializeComponent();
            _user = User.Instance;
            HandleLoginStatus();
        }


        /// <summary>
        /// Change visual appearance of window depending on the login status of the user
        /// </summary>
        private void HandleLoginStatus()
        {
            // Handle connection on Main Menu
            if (_user.IsConnected)
            {
                //hide unregistered panel and show registred one
                UnregisteredPanel.Visibility = Visibility.Collapsed;
                RegisteredPanel.Visibility = Visibility.Visible;
            }
            else
            {
                // If logged out
                UnregisteredPanel.Visibility = Visibility.Visible;
                RegisteredPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RequestChangeState(Enums.States.Register);
        }
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            RequestChangeState(Enums.States.Profile);
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            RequestChangeState(Enums.States.Login);
        }

        private void RequestChangeState(Enums.States state)
        {
            EventManager.Instance.Notice(new ChangeStateEvent() { NextState = state });
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Load.LoadOnLogout();
            RequestChangeState(Enums.States.MainMenu);
        }
    }
}
