using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FrontEnd.Player;
using FrontEnd.Stats;
using FrontEndAccess;
using FrontEndAccess.APIAccess;
using Models;

namespace FrontEnd.Achievements
{
    public class AchievementManager
    {
        public static Dictionary<EnumsModel.Achievement, bool> CurrentUserAchievements { get; set; } = new Dictionary<EnumsModel.Achievement, bool>();

        public static void LogoutReinitialize()
        {
            CurrentUserAchievements = new Dictionary<EnumsModel.Achievement, bool>();
        }

        #region Music Load achievement
        public static void AchieveMusicLoad()
        {
            MainWindow.Achievement.ShowAchievement(Properties.Resources.Music, false);
        }
        #endregion

        #region Achievement 
        public static void AchieveLogin()
        {
            CheckAchievement(EnumsModel.Achievement.FirstTimeConnect, Properties.Resources.Login_A);
        }

        public static void AchieveAvatarChange()
        {
            CheckAchievement(EnumsModel.Achievement.AddAvatar, Properties.Resources.Avatar_A);
        }

        public static void Achieve10000FastGamePoints()
        {
            CheckAchievement(EnumsModel.Achievement.FastGamePoints, Properties.Resources.Fast_A);
        }

        public static void Achieve10000GamePoints()
        {
            CheckAchievement(EnumsModel.Achievement.GamePoints, Properties.Resources.Game_A);
        }

        public static void AchieveFirstOnlineGame()
        {
            CheckAchievement(EnumsModel.Achievement.FirstOnlineGame, Properties.Resources.Network_A);
        }

        public static void AchieveFirstMapCreated()
        {
            CheckAchievement(EnumsModel.Achievement.FirstMapCreated, Properties.Resources.Map_A);
        }

        public static void AchieveFirstOnlineGameWon()
        {
            CheckAchievement(EnumsModel.Achievement.FirstOnlineGameWon, Properties.Resources.Win_A);
        }

        public static void AchievePlayFriendMap()
        {
            CheckAchievement(EnumsModel.Achievement.PlayWithAFriend, Properties.Resources.Friend_A);
        }

        public static void AchieveCampainDone()
        {
            CheckAchievement(EnumsModel.Achievement.FinishCampain, Properties.Resources.Campain_A);
        }
        #endregion

        #region Progress achievement
        public static void AchieveDuchessRank()
        {
            MainWindow.Achievement.ShowAchievement(Properties.Resources.CrownDuchess);
        }
        public static void AchievePrincessRank()
        {
            MainWindow.Achievement.ShowAchievement(Properties.Resources.CrownPrincess);
        }
        public static void AchieveQueenRank()
        {
            MainWindow.Achievement.ShowAchievement(Properties.Resources.CrownPrincess);
        }
        #endregion

        private static void CheckAchievement(EnumsModel.Achievement achievement, Bitmap image)
        {
            if (CurrentUserAchievements != null &&
                CurrentUserAchievements.ContainsKey(achievement) &&
                !CurrentUserAchievements[achievement])
            {
                // trigger Achievement
                ProgressManager.TriggerProgress(ProgressManager.ProgressType.DoneAchievement);
                MainWindow.Achievement.ShowAchievement(image);
                CurrentUserAchievements[achievement] = true;
                CheckForAllAchievements();
                StatsManager.UpdateAchievementsUnlocked();

                // Sync achievements
                ProfileAccess.Instance.UpdateUserProfile(Profile.Instance.CurrentProfile);


                // Post on wall
                if (!string.IsNullOrWhiteSpace(User.Instance.FacebookToken))
                {
                    FacebookAccess.PostOnWall(User.Instance.FacebookToken,achievement);
                }
            }
        }
        private static void CheckForAllAchievements()
        {
            if (CurrentUserAchievements != null &&
                CurrentUserAchievements.ContainsKey(EnumsModel.Achievement.FinishOtherSucess) &&
                !CurrentUserAchievements[EnumsModel.Achievement.FinishOtherSucess])
            {
                var total = CurrentUserAchievements.Count - 1;
                var unlocked = CurrentUserAchievements.Select(x => x.Value).ToList().Count;
                if (total == unlocked)
                {
                    MainWindow.Achievement.ShowAchievement(Properties.Resources.Achievement_A);
                    CurrentUserAchievements[EnumsModel.Achievement.FinishOtherSucess] = true;
                }
            }
        }
    }
}
