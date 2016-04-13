using System.Collections.Generic;
using System.Threading.Tasks;
using FrontEnd.Achievements;
using FrontEnd.Player;
using FrontEndAccess.APIAccess;

namespace FrontEnd.Game.Config.Helper
{
    public static class GameAchievementHelper
    {
        public static void CheckForAchievementTask(List<Zone> zones)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var zone in zones)
                {
                    DoCheckForAchievement(zone);
                }
            });
        }

        public static void DoCheckForAchievement(Zone zone)
        {
            var model = ZoneAccess.Instance.GetMapFromId(zone.HashId);
            if (model.CreatorhashId != Profile.Instance.CurrentProfile.UserHashId)
            {
                AchievementManager.AchievePlayFriendMap();
            }
        }
    }
}