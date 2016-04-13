using System.Linq;
using System.Net;
using Helper.Http;
using Models.Database;
using Models.Token;
using PrincessAPI.Infrastructure;
using static Models.Database.LeaderboardModelHelper;

namespace PrincessAPI.Services
{
    public class LeaderboardService : AbstractService
    {
        public LeaderboardService(UserToken userToken) : base(userToken)
        {
        }

        public LeaderboardModel GetLeaderboard(string zoneHashId)
        {
            if (string.IsNullOrWhiteSpace(zoneHashId))
                throw HttpResponseExceptionHelper.Create("Empty map hash id specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var leaderboard = db.Leaderboards.FirstOrDefault(x => x.ZoneHashId == zoneHashId);

                if (leaderboard == null)
                    throw HttpResponseExceptionHelper.Create("No leaderboard is linked to the specified map hash id",
                        HttpStatusCode.BadRequest);

                return leaderboard;
            }
        }

        public void AddLeaderEntry(string zoneHashId, LeaderModel leader)
        {
            if (string.IsNullOrWhiteSpace(zoneHashId))
                throw HttpResponseExceptionHelper.Create("Empty map hash id specified", HttpStatusCode.BadRequest);

            if (!LeaderModelHelper.IsValid(leader))
                throw HttpResponseExceptionHelper.Create("Invalid leader entry specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var leaderboard = db.Leaderboards.FirstOrDefault(x => x.ZoneHashId == zoneHashId);

                if (leaderboard == null)
                    throw HttpResponseExceptionHelper.Create("Invalid map hash id provided. No related map.",
                        HttpStatusCode.BadRequest);

                leaderboard.Leaders = GetLeaderString(InsertLeader(leaderboard, leader));
                db.SaveChanges();
            }
        }
    }
}