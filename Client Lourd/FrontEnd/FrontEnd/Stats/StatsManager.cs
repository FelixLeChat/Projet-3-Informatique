using System;
using System.Collections.Generic;
using System.Linq;
using FrontEnd.Achievements;
using Models;

namespace FrontEnd.Stats
{
    public class StatsManager
    {
        public static Dictionary<EnumsModel.Stats, int> CurrentUserStats { get; set; }

        public static void LogoutReinitialize()
        {
            CurrentUserStats = null;
        }

        public static void AddGamePoints(int value)
        {
            if (value > 0 && CurrentUserStats != null && CurrentUserStats.ContainsKey(EnumsModel.Stats.TotalPoints))
                CurrentUserStats[EnumsModel.Stats.TotalPoints] += value;
        }

        public static void AddGameTime(int gameSecond)
        {
            if (gameSecond > 0 && CurrentUserStats != null && CurrentUserStats.ContainsKey(EnumsModel.Stats.TotalTimePlayed))
                CurrentUserStats[EnumsModel.Stats.TotalTimePlayed] += gameSecond;
        }

        public static void AddMapCreated()
        {
            // Daily
            DailyManager.AchieveDaily(EnumsModel.DailyType.CreateMap);

            if (CurrentUserStats != null && CurrentUserStats.ContainsKey(EnumsModel.Stats.TotalMapCreated))
                CurrentUserStats[EnumsModel.Stats.TotalMapCreated]++;
        }

        public static void AddGameWon()
        {
            if (CurrentUserStats != null && CurrentUserStats.ContainsKey(EnumsModel.Stats.TotalGameWon))
                CurrentUserStats[EnumsModel.Stats.TotalGameWon]++;
        }

        public static void AddGamePlayed()
        {
            if (CurrentUserStats != null && CurrentUserStats.ContainsKey(EnumsModel.Stats.TotalGamePlayed))
                CurrentUserStats[EnumsModel.Stats.TotalGamePlayed]++;
        }

        public static void UpdateAchievementsUnlocked()
        {
            if (CurrentUserStats == null || !CurrentUserStats.ContainsKey(EnumsModel.Stats.SucessUnlocked)) return;

            var achievements = AchievementManager.CurrentUserAchievements;
            if (achievements == null) return;

            var total = achievements.Count > 0 ? achievements.Count : 1;
            var achieved = achievements.Where(x => x.Value).ToList().Count;
            CurrentUserStats[EnumsModel.Stats.SucessUnlocked] = (int)(((float)achieved/total)*100);
        }
    }
}
