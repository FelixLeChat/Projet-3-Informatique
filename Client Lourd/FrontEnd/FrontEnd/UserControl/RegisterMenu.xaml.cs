using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using FrontEnd.AsyncLoading;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEndAccess;
using FrontEndAccess.APIAccess;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    public partial class RegisterMenu
    {
        private static EventManager _eventManager;
        private static UserAccess _userAccess;
        private static User _user;

        public RegisterMenu()
        {
            InitializeComponent();
            _eventManager = EventManager.Instance;
            _userAccess = UserAccess.Instance;
            _user = User.Instance;

            // Change Title
            MainWindow.Instance.SwitchTitle("Enregistrement");
        }

        #region Passbox tip
        private void PasswordEntryTip_GotFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            if (box != null)
            {
                box.Visibility = Visibility.Hidden;
                PasswordEntry.Focus();
            }
        }
        private void PasswordEntry_LostFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            if (box != null)
            {
                if (string.IsNullOrWhiteSpace(box.Password))
                    PasswordEntryTip.Visibility = Visibility.Visible;
            }
        }
        private void PasswordReentryTip_GotFocus(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            if (box != null)
            {
                box.Visibility = Visibility.Hidden;
                PasswordReentry.Focus();
            }
        }
        private void PasswordEntry_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordEntryTip.Visibility = Visibility.Hidden;
        }
        private void PasswordReentry_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordReentryTip.Visibility = Visibility.Hidden;
        }
        private void PasswordReentry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                RegisterButton_Click(sender, new RoutedEventArgs());
        }
        #endregion

        #region State Change
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }
        #endregion

        #region Online Registration
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            var username = UsernameEntry.Text;
            var password = PasswordEntry.Password;

            // Call in new thread to liberate UI
            var thread = new Thread(() =>
            {
                MainWindow.Instance.PanelLoading = true;
                MainWindow.Instance.PanelMainMessage = "Enregistrement en cours";
                try
                {
                    var token = _userAccess.Register(username, password);
                    _user.UserToken = token;
                    _user.Name = username;
                    _user.IsConnected = true;

                    // Call on main thread
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
                    {
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

        private void LogInWithFacebookButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.PanelLoading = true;
            MainWindow.Instance.PanelMainMessage = "Enregistrement en cours";

            var dialog = new FacebookLoginWindow() { AppId = "719645478171450" };
            if (dialog.ShowDialog() == true)
            {
                // Get the access token from Facebook           
                _user.FacebookToken = dialog.AccessToken;
                _user.PlayerLoginType = User.LoginType.Facebook;

                // Login with facebook credentials
                var inputDialog = new ChatCanalInputWindow("Please enter your username:");
                if (inputDialog.ShowDialog() == true)
                {
                    var username = inputDialog.Answer;

                    var userId = _userAccess.GetFacebookId(_user.FacebookToken);
                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        ShowError("Facebook User is invalid - frontend");
                        return;
                    }
                    
                    try
                    {
                        var token = _userAccess.Register(username, "", userId);
                        _user.UserToken = token;
                        _user.FacebookId = userId;
                        _user.Name = username;
                        _user.IsConnected = true;

                        // Go to main menu if login is completed
                        Load.LoadOnLogin();
                        MainWindow.Instance.PanelLoading = false;
                        _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });


                        FacebookAccess.PostOnWall(_user.FacebookToken, "Je me suis enregistrer a Princess Love balls (Projet 3 informatique)");
                    }
                    catch (Exception exception)
                    {
                        ShowError(exception.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Validate fields needed to send a valid registration
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            ResetError();
            var error = string.Empty;

            // Check if username is correct
            var username = UsernameEntry.Text;
            if (string.IsNullOrWhiteSpace(username))
                error += "Username field must be populated" + Environment.NewLine;

            // Check if password match
            var pass1 = PasswordEntry.Password;
            var pass2 = PasswordReentry.Password;

            if (string.IsNullOrWhiteSpace(pass1))
                error += "Password field must be populated" + Environment.NewLine;
            else if (pass1 != pass2)
                error += "Password must match" + Environment.NewLine;

            if(!string.IsNullOrEmpty(error))
                ShowError(error);

            return string.IsNullOrEmpty(error);
        }

        private void ShowError(string error)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
            {
                MainWindow.Instance.PanelLoading = false;
                if (error.Length > 300)
                    error = "Erreur de serveur, il est en redémarage.";
                MessageBox.Show(error);
            }));
        }

        private void ResetError()
        {
        }

        #endregion

        
    }
}
