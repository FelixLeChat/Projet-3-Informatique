using System.Web.Http;
using Models.Database;
using PrincessAPI.Controllers.SecureControllerHelper;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers
{
    [SecureAPI]
    [RoutePrefix("api/leaderboard")]
    public class LeaderboardController : SecureController
    {
        private readonly LeaderboardService _leaderboardService;

        public LeaderboardController()
        {
            _leaderboardService = new LeaderboardService(UserToken);
        }

        /// <summary>
        /// Get the leaderboard on the given zone. Contain the top 10 player scores
        /// </summary>
        /// <param name="zoneHashId">Id of the Zone</param>
        /// <returns>Information on the leaderboard</returns>
        [HttpGet]
        [Route("{zoneHashId}")]
        public LeaderboardModel GetLeaderboard(string zoneHashId)
        {
            return _leaderboardService.GetLeaderboard(zoneHashId);
        }

        /// <summary>
        /// Add an entry on the leaderboard of the zone
        /// </summary>
        /// <param name="zoneHashId">Id on the Zone</param>
        /// <param name="leader">Information on the new score</param>
        [HttpPost]
        [Route("{zoneHashId}")]
        public void AddLeaderEntry(string zoneHashId, LeaderModel leader)
        {
            _leaderboardService.AddLeaderEntry(zoneHashId, leader);
        }
    }
}
