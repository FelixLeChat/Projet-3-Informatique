using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEnd.Setting;
using FrontEnd.UserControl.Tutorial;
using FrontEnd.ViewModel;
using Helper.Image;
using Models;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu
    {
        private readonly EventManager _eventManager;
        public MainMenu()
        {
            InitializeComponent();
            _eventManager = EventManager.Instance;

            // Load Options
            var ins = Options.Instance;

            InitializePrincess();

            // Change Title
            MainWindow.Instance.SwitchTitle("Menu Principal");
        }


        #region State Transition
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.Instance.IsConnected)
            {
                if (Profile.Instance.CurrentProfile.ShowGameTutorial)
                    TutorialHelper.StartGameTutorial();
            }
            else
            {
                if (Properties.Settings.Default.ShowGameTutorial)
                    TutorialHelper.StartGameTutorial();
            }
            
            RequestChangeState(Enums.States.GameMenu);
        }
        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            RequestChangeState(Enums.States.Options);
        }
        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.Instance.IsConnected)
            {
                if(Profile.Instance.CurrentProfile.ShowEditorTutorial)
                    TutorialHelper.StartEditorTutorial();
            }
            else
            {
                if(Properties.Settings.Default.ShowTutorial)
                    TutorialHelper.StartEditorTutorial();
            }
            RequestChangeState(Enums.States.Edition);
        }
        private void RequestChangeState(Enums.States state)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = state});
        }

        #endregion

        public void InitializePrincess()
        {
            if (User.Instance.IsConnected)
            {
                Bitmap bitmap = null;
                switch (Profile.Instance.CurrentProfile.Picture)
                {
                    case EnumsModel.PrincessAvatar.Ariel:
                        bitmap = Properties.Resources.ariel_full;
                        break;
                    case EnumsModel.PrincessAvatar.Belle:
                        bitmap = Properties.Resources.belle_full;
                        break;
                    case EnumsModel.PrincessAvatar.Cinder:
                        bitmap = Properties.Resources.cinderella_full;
                        break;
                    case EnumsModel.PrincessAvatar.Frog:
                        bitmap = Properties.Resources.tiana_full;
                        break;
                    case EnumsModel.PrincessAvatar.Jasmine:
                        bitmap = Properties.Resources.jasmine_full;
                        break;
                    case EnumsModel.PrincessAvatar.Mulan:
                        bitmap = Properties.Resources.mulan_full;
                        break;
                    case EnumsModel.PrincessAvatar.Poca:
                        bitmap = Properties.Resources.pocahontas_full;
                        break;
                    case EnumsModel.PrincessAvatar.Ray:
                        bitmap = Properties.Resources.raiponce_full;
                        break;
                    case EnumsModel.PrincessAvatar.Sleep:
                        bitmap = Properties.Resources.aurora_full;
                        break;
                    case EnumsModel.PrincessAvatar.Snow:
                        bitmap = Properties.Resources.snowwhite_full;
                        break;
                }

                if(bitmap != null)
                    PrincessImage.Source = ImageHelper.LoadBitmap(bitmap);
            }
        }

        private void QuitButton_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Instance.Close();
        }
    }
}
