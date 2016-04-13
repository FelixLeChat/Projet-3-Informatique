using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.Database
{
    public class ProfileModel
    {
        public int Id { get; set; }
        public string UserHashId { get; set; }
        public string Username { get; set; }
        public EnumsModel.PrincessAvatar Picture { get; set; }
        public string Description { get; set; }
        public EnumsModel.PrincessTitle PrincessTitle { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public bool ShowEditorTutorial { get; set; } = true;
        public bool ShowGameTutorial { get; set; } = true;
        public bool ShowLightTutorial { get; set; } = true;
        public bool IsPrivate { get; set; }
        public string AchievementsString { get; set; }
        public string StatsString { get; set; }
        public string FriendsHashIds { get; set; }
        public string MapsHashIds { get; set; }
    }

    public class ProfileModelHelper
    {
        public static ProfileModel Initialize(ProfileModel newProfile)
        {
            // initialise Achievements
            newProfile.AchievementsString = JsonConvert.SerializeObject(InitAchievements());

            // Initialise Stats
            newProfile.StatsString = JsonConvert.SerializeObject(InitStats());

            newProfile.Description = "No description set";
            newProfile.Picture = EnumsModel.PrincessAvatar.Default;
            newProfile.PrincessTitle = EnumsModel.PrincessTitle.Lady;

            return newProfile;
        }

        private static Dictionary<EnumsModel.Achievement, bool> InitAchievements()
        {
            var achievements = new Dictionary<EnumsModel.Achievement, bool>
            {
                {EnumsModel.Achievement.AddAvatar, false},
                {EnumsModel.Achievement.FastGamePoints, false},
                {EnumsModel.Achievement.FinishCampain, false},
                {EnumsModel.Achievement.FinishOtherSucess, false},
                {EnumsModel.Achievement.FirstMapCreated, false},
                {EnumsModel.Achievement.FirstOnlineGame, false},
                {EnumsModel.Achievement.FirstOnlineGameWon, false},
                {EnumsModel.Achievement.FirstTimeConnect, false},
                {EnumsModel.Achievement.GamePoints, false},
                {EnumsModel.Achievement.PlayWithAFriend, false}
            };

            return achievements;
        }

        public static string AchieveAll()
        {
            var achievements = new Dictionary<EnumsModel.Achievement, bool>
            {
                {EnumsModel.Achievement.AddAvatar, true},
                {EnumsModel.Achievement.FastGamePoints, true},
                {EnumsModel.Achievement.FinishCampain, true},
                {EnumsModel.Achievement.FinishOtherSucess, true},
                {EnumsModel.Achievement.FirstMapCreated, true},
                {EnumsModel.Achievement.FirstOnlineGame, true},
                {EnumsModel.Achievement.FirstOnlineGameWon, true},
                {EnumsModel.Achievement.FirstTimeConnect, true},
                {EnumsModel.Achievement.GamePoints, true},
                {EnumsModel.Achievement.PlayWithAFriend, true}
            };

            return JsonConvert.SerializeObject(achievements);
        }

        private static Dictionary<EnumsModel.Stats, int> InitStats()
        {
            var stats = new Dictionary<EnumsModel.Stats, int>
            {
                {EnumsModel.Stats.SucessUnlocked, 0},
                {EnumsModel.Stats.TotalGamePlayed, 0},
                {EnumsModel.Stats.TotalGameWon, 0},
                {EnumsModel.Stats.TotalMapCreated, 0},
                {EnumsModel.Stats.TotalPoints, 0},
                {EnumsModel.Stats.TotalTimePlayed, 0}
            };
            return stats;
        }

        public static Dictionary<EnumsModel.Achievement, bool> GetAchievements(ProfileModel profile)
        {
            if (string.IsNullOrWhiteSpace(profile.AchievementsString))
                return InitAchievements();
            return JsonConvert.DeserializeObject<Dictionary<EnumsModel.Achievement, bool>>(profile.AchievementsString);
        }

        public static Dictionary<EnumsModel.Stats, int> GetStats(ProfileModel profile)
        {
            if (string.IsNullOrWhiteSpace(profile.StatsString))
                return InitStats();
            return JsonConvert.DeserializeObject<Dictionary<EnumsModel.Stats, int>>(profile.StatsString);
        }
    }
}