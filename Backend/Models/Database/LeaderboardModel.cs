using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models.Database
{
    public class LeaderboardModel
    {
        public int Id { get; set; }
        public string ZoneHashId { get; set; }
        public string Leaders { get; set; }
    }

    public class LeaderboardModelHelper
    {
        public static List<LeaderModel> GetOrderedLeaders(LeaderboardModel leaderboard)
        {
            var leaders = new List<LeaderModel>();
            if (string.IsNullOrWhiteSpace(leaderboard?.Leaders))
                return leaders;

            return JsonConvert.DeserializeObject<List<LeaderModel>>(leaderboard.Leaders).OrderByDescending(x => x.Points).ToList();
        }

        public static List<LeaderModel> InsertLeader(LeaderboardModel leaderboard, LeaderModel leader)
        {
            var list = GetOrderedLeaders(leaderboard);
            list.Add(leader);
            list = list.OrderByDescending(x => x.Points).ToList();

            if (list.Count > 10)
                return list.Take(10).ToList();
            return list;
        }

        public static string GetLeaderString(List<LeaderModel> leaders)
        {
            return JsonConvert.SerializeObject(leaders);
        }
    }
}
