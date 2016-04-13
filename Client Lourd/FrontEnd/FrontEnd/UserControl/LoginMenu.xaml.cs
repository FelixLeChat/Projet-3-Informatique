using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using FrontEnd.AsyncLoading;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEnd.Properties;
using FrontEndAccess.APIAccess;
using Helper.Jwt;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    public partial class LoginMenu
    {
        private readonly EventManager _eventManager;
        private readonly UserAccess _userAccess;
        private readonly User _user;

        public LoginMenu()
        {
            InitializeComponent();
            _eventManager = EventManager.Instance;
            _user = User.Instance;
            _userAccess = UserAccess.Instance;

            // handle Remembre-me
            if (Settings.Default.RememberMe)
            {
                RememberMeCheckbox.IsChecked = true;
                UsernameEntry.Text = Settings.Default.Username;
                PasswordEntry.Password = Settings.Default.Password;
                PasswordEntryTip.Visibility = Visibility.Hidden;
                LoginButton.Focus();
            }

            // Change Title
            MainWindow.Instance.SwitchTitle("Connexion");
        }
        
        #region PassboxTip
        private void PasswordEntryTip_GotFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            if (box == null) return;
            box.Visibility = Visibility.Hidden;
            PasswordEntry.Focus();
        }
        private void PasswordEntry_LostFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            if (box == null) return;
            if (string.IsNullOrWhiteSpace(box.Password))
                PasswordEntryTip.Visibility = Visibility.Visible;
        }
        private void PasswordEntry_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordEntryTip.Visibility = Visibility.Hidden;
        }
        private void PasswordEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LoginButton_Click(sender, new RoutedEventArgs());
        }
        #endregion

        #region State Transition
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }
        #endregion

        #region Login
        /// <summary>
        /// handle Login with Facebook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogInWithFacebookButton_Click(object sender, RoutedEventArgs e)
        {
            DisableLoginButtons();
            MainWindow.Instance.PanelLoading = true;
            MainWindow.Instance.PanelMainMessage = "Identification en cours";

            var dialog = new FacebookLoginWindow() { AppId = "719645478171450" };
            if (dialog.ShowDialog() == true)
            {
                if (string.IsNullOrWhiteSpace(dialog.AccessToken))
                {
                    ShowError("La fenêtre Facebook a été fermé avant la connexion.");
                    return;
                }

                // Get the access token from Facebook           
                _user.FacebookToken = dialog.AccessToken;
                _user.PlayerLoginType = User.LoginType.Facebook;

                var userId = _userAccess.GetFacebookId(_user.FacebookToken);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    ShowError("L'utilisateur Facebook est invalide");
                    return;
                }

                // Login with facebook credentials
                try
                {
                    var token = _userAccess.Login("", "", userId);
                    var userToken = JwtHelper.DecodeToken(token);
                    _user.FacebookId = userId;
                    _user.UserToken = token;
                    _user.Name = userToken.Username;
                    _user.IsConnected = true;

                    // Go to main menu if login is completed
                    Load.LoadOnLogin();
                    MainWindow.Instance.PanelLoading = false;
                    _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
                }
                catch (Exception exception)
                {
                    ShowError(exception.Message);
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            var username = UsernameEntry.Text;
            var password = PasswordEntry.Password;
            if (RememberMeCheckbox.IsChecked != null)
                Settings.Default.RememberMe = (bool)RememberMeCheckbox.IsChecked;

            // check for remember me
            if (Settings.Default.RememberMe)
            {
                Settings.Default.Username = username;
                Settings.Default.Password = password;
            }

            DisableLoginButtons();

            // Call in new thread to liberate UI
            var thread = new Thread(() =>
            {
                MainWindow.Instance.PanelLoading = true;
                MainWindow.Instance.PanelMainMessage = "Identification en cours";
                // All entry are valid, we can proceed with login
                try
                {
                    var token = _userAccess.Login(username, password);
                    _user.UserToken = token;
                    _user.Name = username;
                    _user.IsConnected = true;

                    var id = JwtHelper.DecodeToken(token).UserId;
                    NativeFunction.connexion(new StringBuilder(token), token.Length,new StringBuilder(id), id.Length); // Todo kf, add userId

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
                    {
                        // Go to main menu if login is completed
                        _eventManager.Notice(new ChangeStateEvent() {NextState = Enums.States.MainMenu});
                        MainWindow.Instance.PanelLoading = false;
                        Load.LoadOnLogin();
                    }));
                }
                catch (Exception exception)
                {
                    ShowError(exception.Message);
                }
            });
            thread.Start();
        }

        private bool ValidateFields()
        {
            ResetError();
            var error = string.Empty;

            // Check if username is correct
            var username = UsernameEntry.Text;
            if (string.IsNullOrWhiteSpace(username))
                error += "Le champ utilisateur doit être populer " + Environment.NewLine;

            // Check if password is not null
            var pass1 = PasswordEntry.Password;

            if (string.IsNullOrWhiteSpace(pass1))
                error += "Le champ mot de passe doit être populer" + Environment.NewLine;

            if (!string.IsNullOrEmpty(error))
                ShowError(error);

            return string.IsNullOrEmpty(error);
        }

        private void ShowError(string error)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
            {
                EnableloginButton();
                if (error.Length > 300)
                    error = "Erreur de serveur, il est en redémarage.";
                MessageBox.Show(error);
                MainWindow.Instance.PanelLoading = false;
            }));
        }

        private void ResetError()
        {
        }

        private void DisableLoginButtons()
        {
            LoginButton.IsEnabled = false;
            LogInWithFacebookButton.IsEnabled = false;
        }

        private void EnableloginButton()
        {
            LoginButton.IsEnabled = true;
            LogInWithFacebookButton.IsEnabled = true;
        }
        #endregion
    }
}
