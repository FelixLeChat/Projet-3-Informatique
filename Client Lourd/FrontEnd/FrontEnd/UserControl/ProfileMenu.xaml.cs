using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using FrontEnd.Achievements;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using FrontEnd.Stats;
using FrontEndAccess.APIAccess;
using Helper.Image;
using Models;
using Models.Database;
using EventManager = FrontEnd.Core.EventManager;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace FrontEnd.UserControl
{
    /// <summary>
    /// Interaction logic for ProfileMenu.xaml
    /// </summary>
    public partial class ProfileMenu
    {
        private static EventManager _eventManager;
        private static ProfileAccess _profileAccess;
        private static ProfileModel _profile;
        private static bool _friendProfile = true;

        public ProfileMenu(ProfileModel userProfile = null)
        {
            _friendProfile = true;
            InitializeComponent();
            _eventManager = EventManager.Instance;
            _profileAccess = ProfileAccess.Instance;
            _profile = userProfile;


            // get current user profile information from the web
            if (Profile.Instance.CurrentProfile == null)
            {
                Profile.Instance.CurrentProfile = _profileAccess.GetUserProfile();
            }

            // take current user profile if no profile is providen
            if (userProfile == null)
            {
                _profile = Profile.Instance.CurrentProfile;
                _friendProfile = false;
            }
            SetupProfile();

            // Change Title
            MainWindow.Instance.SwitchTitle("Profile");
        }

        #region State change
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.MainMenu });
        }
        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.ProfileEdit });
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.UserFriends });
        }
        private void MyZones_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.PanelLoading = true;
            MainWindow.Instance.PanelMainMessage = "Loading Zones";
            _eventManager.Notice(new ChangeStateEvent() { NextState = Enums.States.UserZones });
        }
        private void BackButton_Click(object sender, MouseButtonEventArgs e)
        {
            Program.MainWindow.SwitchScreen(new UserFriendsMenu());
        }
        #endregion

        #region Initialize profile
        private void SetupProfile()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
            {
                // enter information from _currentProfile
                if(_profile.Description.Length > 100)
                    Description.Text = _profile.Description.Substring(0,100)+" ... ";
                else
                    Description.Text = _profile.Description;
                Username.Content = _profile.Username;

                // Title
                var princessRank = _profile.PrincessTitle;
                PrincessRank.Content = _profile.PrincessTitle.ToString();
                InitImageRectangle(GetRankBitmap(princessRank), PrincessRankImg);

                // Set stats
                var stats = new Dictionary<EnumsModel.Stats, int>();
                if (_friendProfile)
                    stats = ProfileModelHelper.GetStats(_profile);
                else
                    stats = StatsManager.CurrentUserStats;

                if (stats.ContainsKey(EnumsModel.Stats.SucessUnlocked))
                    Sucessunlocked.Content = stats[EnumsModel.Stats.SucessUnlocked] + " %";
                if (stats.ContainsKey(EnumsModel.Stats.TotalGamePlayed))
                    GamePlayed.Content = stats[EnumsModel.Stats.TotalGamePlayed];
                if (stats.ContainsKey(EnumsModel.Stats.TotalGameWon))
                    GameWon.Content = stats[EnumsModel.Stats.TotalGameWon];
                if (stats.ContainsKey(EnumsModel.Stats.TotalMapCreated))
                    MapCreated.Content = stats[EnumsModel.Stats.TotalMapCreated];
                if (stats.ContainsKey(EnumsModel.Stats.TotalPoints))
                    Points.Content = stats[EnumsModel.Stats.TotalPoints];
                if (stats.ContainsKey(EnumsModel.Stats.TotalTimePlayed))
                    TimeInGame.Content = stats[EnumsModel.Stats.TotalTimePlayed];

                // Set Achievements
                Dictionary<EnumsModel.Achievement, bool> achievements = null;
                if (_friendProfile)
                    achievements = ProfileModelHelper.GetAchievements(_profile);
                else
                    achievements = AchievementManager.CurrentUserAchievements ?? new Dictionary<EnumsModel.Achievement, bool>();

                if(achievements.ContainsKey(EnumsModel.Achievement.FirstTimeConnect) && achievements[EnumsModel.Achievement.FirstTimeConnect])
                    LoginImageLock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.AddAvatar) && achievements[EnumsModel.Achievement.AddAvatar])
                    AvatarImageLock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.FastGamePoints) && achievements[EnumsModel.Achievement.FastGamePoints])
                    FastGameLock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.GamePoints) && achievements[EnumsModel.Achievement.GamePoints])
                    GameLock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.FirstOnlineGame) && achievements[EnumsModel.Achievement.FirstOnlineGame])
                    NetworkLock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.FirstMapCreated) && achievements[EnumsModel.Achievement.FirstMapCreated])
                    Maplock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.FirstOnlineGameWon) && achievements[EnumsModel.Achievement.FirstOnlineGameWon])
                    GameWonOnceLock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.PlayWithAFriend) && achievements[EnumsModel.Achievement.PlayWithAFriend])
                    FriendZoneLock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.FinishCampain) && achievements[EnumsModel.Achievement.FinishCampain])
                    CampainDoneLock.Visibility = Visibility.Collapsed;
                if (achievements.ContainsKey(EnumsModel.Achievement.FinishOtherSucess) && achievements[EnumsModel.Achievement.FinishOtherSucess])
                    AchievementAllLock.Visibility = Visibility.Collapsed;

                // Profile Picture
                var profilePic = _profile.Picture;
                ProfileImg.Source = ImageHelper.LoadBitmap(GetAvatarBitmap(profilePic));

                // Public / Private
                InitImageRectangle(GetPrivateBitmap(_profile.IsPrivate), PrivateImage);

                // Experience
                var experience = "Expérience : " + _profile.Experience + "/" + ProgressManager.LevelExperience[_profile.Level];
                PlayerExperience.Content = experience;

                // Daily
                if (_friendProfile)
                {
                    DailyDiv.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DailyDiv.Visibility = Visibility.Visible;
                    DailyText.Text = "Le défis journalier est de : " +
                                     DailyManager.GetDailyDescription(Profile.Instance.Daily.DailyType);
                    DailyDoneImage.Visibility = Profile.Instance.Daily.IsDone ? Visibility.Visible : Visibility.Collapsed;
                }

                // No modif if not our profile
                if (_friendProfile)
                {
                    UserProfileButtons.Visibility = Visibility.Collapsed;
                    SeeFriendsButton.Visibility = Visibility.Collapsed;
                    FriendProfileButtons.Visibility = Visibility.Visible;
                }
                else
                {
                    UserProfileButtons.Visibility = Visibility.Visible;
                    SeeFriendsButton.Visibility = Visibility.Visible;
                    FriendProfileButtons.Visibility = Visibility.Collapsed;
                }

            }));
        }

        private void InitImageRectangle(Bitmap bitmap, Rectangle rectangle)
        {
            rectangle.Fill = ImageHelper.GetImageBrush(bitmap);
        }

        private Bitmap GetAvatarBitmap(EnumsModel.PrincessAvatar avatar)
        {
            return EnumToImage.GetAvatarBitmap(avatar);
        }

        private Bitmap GetRankBitmap(EnumsModel.PrincessTitle title)
        {
            return EnumToImage.GetRankBitmap(title);
        }

        private Bitmap GetPrivateBitmap(bool isPrivate)
        {
            if (isPrivate)
                return Properties.Resources.lock2;
            else
                return Properties.Resources.unlock2;
        }
        #endregion

        private void AchievementCarrousel_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedIndex = AchievementCarrousel.SelectedIndex;

            if (selectedIndex >= 0)
            {
                AchievementDescription.Text = EnumsModel.GetAchievementDescription((EnumsModel.Achievement)selectedIndex);
            }
        }
    }
}
