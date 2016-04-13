using System.Threading;
using System.Windows;
using FrontEnd.Achievements;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEnd.Stats;
using FrontEndAccess.APIAccess;
using Models;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.UserControl
{
    public partial class ProfileEditMenu
    {
        private readonly EventManager _eventManager;
        private readonly ProfileAccess _profileAccess;
        private readonly Profile _profile;

        public ProfileEditMenu()
        {
            InitializeComponent();
            _eventManager = EventManager.Instance;
            _profileAccess = ProfileAccess.Instance;
            _profile = Profile.Instance;

            InitialiseVisual();

            // Daily
            DailyManager.AchieveDaily(EnumsModel.DailyType.UpdateProfile);

            // Change Title
            MainWindow.Instance.SwitchTitle("Édition Profile");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.Profile });
        }
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (PrincessImageCarrousel.SelectedIndex >= 0)
            {
                _profile.CurrentProfile.Picture = (EnumsModel.PrincessAvatar) PrincessImageCarrousel.SelectedIndex + 1;
                AchievementManager.AchieveAvatarChange();
            }
            _profile.CurrentProfile.Description = Description.Text;
            
            _profile.CurrentProfile.IsPrivate = !(PrivateSlider.Value > 5); ;

            // UpdateWindow profile
            var thread = new Thread(() =>
            {
                _profileAccess.UpdateUserProfile(_profile.CurrentProfile);
            });
            thread.Start();
            
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.Profile });
        }

        private void InitialiseVisual()
        {
            if (_profile.CurrentProfile.Picture != EnumsModel.PrincessAvatar.Default)
            {
                PrincessImageCarrousel.SelectedIndex = (int)_profile.CurrentProfile.Picture - 1;
                AchievementManager.AchieveAvatarChange();
            }

            Description.Text = _profile.CurrentProfile.Description;

            PrivateSlider.Value = _profile.CurrentProfile.IsPrivate ? PrivateSlider.Minimum : PrivateSlider.Maximum;
        }
    }
}
