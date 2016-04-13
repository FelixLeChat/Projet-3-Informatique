using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEnd.Setting;
using FrontEndAccess.APIAccess;
using Application = System.Windows.Application;
using EventManager = FrontEnd.Core.EventManager;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for OptionMenu.xaml
    /// </summary>
    public partial class OptionMenu
    {
        public static OptionMenu Instance;
        private readonly EventManager _eventManager;
        private readonly Options _options;

        public OptionMenu()
        {
            Instance = this;
            _eventManager = EventManager.Instance;
            _options = Options.Instance;

            InitializeComponent();
            SetupOptions();

            // Change Title
            MainWindow.Instance.SwitchTitle("Options");

            if(!User.Instance.IsConnected)
                ResetTutorialBtn.Visibility = Visibility.Hidden;
        }

        #region State Transition
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }
        #endregion

        #region Setting Handling
        public void SetupOptions()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
            {
                // Game
                TextNbBall.Text = _options.BallCount.ToString();
                DoubleBallMode.IsChecked = _options.DoubleBallMode;
                ReboundForceIncreased.IsChecked = _options.IncrReboundForce;

                // Hotkey
                LeftJ1.Content = (Keys) _options.LeftJ1;
                LeftJ2.Content = (Keys) _options.LeftJ2;
                RightJ1.Content = (Keys) _options.RightJ1;
                RightJ2.Content = (Keys) _options.RightJ2;
                Spring.Content = (Keys) _options.SpringKey;

                // Debug
                ShowDebug.IsChecked = _options.ShowDebug;
                ShowLighting.IsChecked = _options.ShowLighting;
                ShowCollisionSpeed.IsChecked = _options.ShowCollisionSpeed;
                ShowBallGen.IsChecked = _options.ShowBallGeneration;
                ShowPortalLimit.IsChecked = _options.ShowPortalAttraction;
            }));
        }
        #endregion

        #region Game Options
        private void DoubleBallMode_Click(object sender, RoutedEventArgs e)
        {
            _options.DoubleBallMode = DoubleBallMode.IsChecked ?? false;
        }
        private void ReboundForceIncreased_Click(object sender, RoutedEventArgs e)
        {
            _options.IncrReboundForce = ReboundForceIncreased.IsChecked ?? false;

        }

        #region Numeric UpDown
        private const int MaxBille = 15;
        private const int MinBille = 1;

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            if(_options.BallCount < MaxBille)
                _options.BallCount++;
            TextNbBall.Text = _options.BallCount.ToString();
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if(_options.BallCount > MinBille)
                _options.BallCount--;
            TextNbBall.Text = _options.BallCount.ToString();
        }
        #endregion
        #endregion

        #region Key Options
        private bool CheckKey { get; set; }

        private void LeftJ1_Click(object sender, RoutedEventArgs e)
        {
            LeftJ1.Content = "press a key";
            CheckKey = true;
        }
        private void RightJ1_Click(object sender, RoutedEventArgs e)
        {
            RightJ1.Content = "press a key";
            CheckKey = true;
        }
        private void LeftJ2_Click(object sender, RoutedEventArgs e)
        {
            LeftJ2.Content = "press a key";
            CheckKey = true;
        }
        private void RightJ2_Click(object sender, RoutedEventArgs e)
        {
            RightJ2.Content = "press a key";
            CheckKey = true;
        }
        private void Spring_Click(object sender, RoutedEventArgs e)
        {
            Spring.Content = "press a key";
            CheckKey = true;
        }

        // Change config on key down
        private void LeftJ1_KeyDown(object sender, KeyEventArgs e)
        {
            if (CheckKey)
            {
                LeftJ1.Content = e.Key;
                _options.LeftJ1 = KeyInterop.VirtualKeyFromKey(e.Key);
                CheckKey = false;
            }
        }
        private void RightJ1_KeyDown(object sender, KeyEventArgs e)
        {
            if (CheckKey)
            {
                RightJ1.Content = e.Key;
                _options.RightJ1 = KeyInterop.VirtualKeyFromKey(e.Key);
                CheckKey = false;
            }
        }
        private void LeftJ2_KeyDown(object sender, KeyEventArgs e)
        {
            if (CheckKey)
            {
                LeftJ2.Content = e.Key;
                _options.LeftJ2 = KeyInterop.VirtualKeyFromKey(e.Key);
                CheckKey = false;
            }
        }
        private void RightJ2_KeyDown(object sender, KeyEventArgs e)
        {
            if (CheckKey)
            {
                RightJ2.Content = e.Key;
                _options.RightJ2 = KeyInterop.VirtualKeyFromKey(e.Key);
                CheckKey = false;
            }
        }
        private void Spring_KeyDown(object sender, KeyEventArgs e)
        {
            if (CheckKey)
            {
                Spring.Content = e.Key;
                _options.SpringKey = KeyInterop.VirtualKeyFromKey(e.Key);
                CheckKey = false;
            }
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            SetupOptions();
        }


        #endregion

        #region Debug Options
        private void ShowDebug_Click(object sender, RoutedEventArgs e)
        {
           _options.ShowDebug = ShowDebug.IsChecked ?? false;
        }
        private void ShowBallGen_Click(object sender, RoutedEventArgs e)
        {
            _options.ShowBallGeneration = ShowBallGen.IsChecked ?? false;
        }
        private void ShowCollisionSpeed_Click(object sender, RoutedEventArgs e)
        {
            _options.ShowCollisionSpeed = ShowCollisionSpeed.IsChecked ?? false;
        }
        private void ShowLighting_Click(object sender, RoutedEventArgs e)
        {
            _options.ShowLighting = ShowLighting.IsChecked ?? false;
        }
        private void ShowPortalLimit_Click(object sender, RoutedEventArgs e)
        {
            _options.ShowPortalAttraction = ShowPortalLimit.IsChecked ?? false;
        }
        #endregion

        #region Buttons
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            _options.UpdateConfigs();
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }

        private void RestoreDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            _options.DefaultConfigs();
            SetupOptions();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _options.GetCurrentConfigs();
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }

        private void ResetTutoriel_Click(object sender, RoutedEventArgs e)
        {

            ProfileAccess.Instance.SetEditorTutorialVisibility(true);

            Profile.Instance.CurrentProfile.ShowEditorTutorial = true;
            ProfileAccess.Instance.SetGameTutorialVisibility(true);

            Profile.Instance.CurrentProfile.ShowGameTutorial = true;
        }
        #endregion
    }
}