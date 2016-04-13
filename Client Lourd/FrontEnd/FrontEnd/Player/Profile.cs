using System.Collections.Generic;
using FrontEnd.Achievements;
using FrontEnd.Stats;
using FrontEndAccess.APIAccess;
using Models;
using Models.Database;
using Newtonsoft.Json;

namespace FrontEnd.Player
{
    public class Profile
    {
        private static Profile _instance;
        public static Profile Instance => _instance ?? (_instance = new Profile());

        private Profile(){}

        private static ProfileModel _profile;
        public ProfileModel CurrentProfile
        {
            get
            {
                if (_profile != null)
                {
                    // always take manager value as last updated one
                    _profile.StatsString = JsonConvert.SerializeObject(StatsManager.CurrentUserStats);
                    _profile.AchievementsString = JsonConvert.SerializeObject(AchievementManager.CurrentUserAchievements);
                    _profile.Experience = ProgressManager.CurrentExperience;
                    _profile.PrincessTitle = ProgressManager.CurrentPrincessTitle;
                    _profile.Level = ProgressManager.CurrentLevel;

                    return _profile;
                }

                if (User.Instance.IsConnected && _profile == null)
                {
                    _profile = ProfileAccess.Instance.GetUserProfile();
                }

                return _profile;
            }
            set
            {
                _profile = value;

                // Set stats on profile modification
                if (!string.IsNullOrWhiteSpace(_profile?.StatsString))
                {
                    StatsManager.CurrentUserStats = ProfileModelHelper.GetStats(_profile);
                }

                // Set Achievement on profile modification
                if (!string.IsNullOrWhiteSpace(_profile?.AchievementsString))
                {
                    AchievementManager.CurrentUserAchievements = ProfileModelHelper.GetAchievements(_profile);
                }

                // Set progression
                ProgressManager.CurrentExperience = _profile?.Experience ?? 0;
                ProgressManager.CurrentPrincessTitle = _profile?.PrincessTitle ?? EnumsModel.PrincessTitle.Lady;
            }
        }

        public DailyModel Daily { get; set; }

        public List<BasicUserInfo> AllUserList { get; set; }
    }
}
