using System.Collections.Generic;
using FrontEnd.Achievements;
using FrontEnd.Player;
using FrontEndAccess.APIAccess;
using Models;

namespace FrontEnd.Stats
{
    public class ProgressManager
    {
        public static EnumsModel.PrincessTitle CurrentPrincessTitle;
        public static int CurrentExperience;
        public static int CurrentLevel;

        public enum ProgressType
        {
            CompleteGame,
            NewZone,
            DoneAchievement
        }

        public static Dictionary<ProgressType,int> ProgressValue = new Dictionary<ProgressType, int>()
        {
            {ProgressType.CompleteGame, 20 },
            {ProgressType.NewZone, 20 },
            {ProgressType.DoneAchievement, 50 }
        };

        public static List<int> LevelExperience = new List<int>()
        {
            100,400,1000,10000,20000,100000000
        };

        public static void TriggerProgress(ProgressType type)
        {
            CurrentExperience += ProgressValue[type];
            CheckExperience();
        }

        private static void CheckExperience()
        {
            if (Profile.Instance.CurrentProfile.PrincessTitle == EnumsModel.PrincessTitle.MASTER)
            {
                CurrentLevel = 3;
                CurrentPrincessTitle = EnumsModel.PrincessTitle.MASTER;
                return;
            }


            if (CurrentExperience >= LevelExperience[2])
            {
                if (CurrentPrincessTitle == EnumsModel.PrincessTitle.Queen) return;

                AchievementManager.AchieveQueenRank();
                CurrentPrincessTitle = EnumsModel.PrincessTitle.Queen;
                CurrentLevel = (int) CurrentPrincessTitle;
            }
            else if (CurrentExperience >= LevelExperience[1])
            {
                if (CurrentPrincessTitle == EnumsModel.PrincessTitle.Princess) return;

                AchievementManager.AchievePrincessRank();
                CurrentPrincessTitle = EnumsModel.PrincessTitle.Princess;
                CurrentLevel = (int)CurrentPrincessTitle;
            }
            else if (CurrentExperience >= LevelExperience[0])
            {
                if (CurrentPrincessTitle == EnumsModel.PrincessTitle.Duchess) return;

                AchievementManager.AchieveDuchessRank();
                CurrentPrincessTitle = EnumsModel.PrincessTitle.Duchess;
                CurrentLevel = (int)CurrentPrincessTitle;
            }
        }

        public static void InitializeRank()
        {
            if (Profile.Instance.CurrentProfile.PrincessTitle == EnumsModel.PrincessTitle.MASTER)
            {
                CurrentLevel = 3;
                CurrentPrincessTitle = EnumsModel.PrincessTitle.MASTER;
                return;
            }

            var level = 0;
            if (CurrentExperience >= LevelExperience[2])
                level = 3;
            else if (CurrentExperience >= LevelExperience[1])
                level = 2;
            else if (CurrentExperience >= LevelExperience[0])
                level = 1;

            CurrentPrincessTitle = (EnumsModel.PrincessTitle) level;
            CurrentLevel = level;

            // Sync old profile level and title
            ProfileAccess.Instance.UpdateUserProfile(Profile.Instance.CurrentProfile);
        }
    }
}
