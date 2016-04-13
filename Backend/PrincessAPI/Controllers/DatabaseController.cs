using System.Linq;
using System.Web.Http;
using Models.Database;
using PrincessAPI.Infrastructure;

namespace PrincessAPI.Controllers
{
    [RoutePrefix("api/database")]
    public class DatabaseController : ApiController
    {
        /// <summary>
        /// Clear friends, zones, dailies, leaderboard from database
        /// </summary>
        [HttpGet]
        [Route("clear")]
        public void ClearDatabase()
        {
            using (var context = new SystemDBContext())
            {
                var felixHash = GetFelixHashId();
                // Delete all Maps
                foreach (var id in context.Maps.Where(x => x.CreatorhashId != felixHash).Select(e => e.Id))
                {
                    var entity = new MapModel() { Id = id };
                    context.Maps.Attach(entity);
                    context.Maps.Remove(entity);
                }
                

                // Delete all Dailies
                foreach (var id in context.Daylies.Where(x => x.UserHashId != felixHash).Select(e => e.Id))
                {
                    var entity = new DailyModel() { Id = id };
                    context.Daylies.Attach(entity);
                    context.Daylies.Remove(entity);
                }

                // Delete all Leaderboards
                foreach (var id in context.Leaderboards.Select(e => e.Id))
                {
                    var entity = new LeaderboardModel() { Id = id };
                    context.Leaderboards.Attach(entity);
                    context.Leaderboards.Remove(entity);
                }

                // Delete all Friends
                foreach (var id in context.Friends.Select(e => e.Id))
                {
                    var entity = new FriendModel() { Id = id };
                    context.Friends.Attach(entity);
                    context.Friends.Remove(entity);
                }

                context.SaveChanges();

            }
        }

        /// <summary>
        /// Clear + User, profile
        /// </summary>
        [HttpGet]
        [Route("clearall")]
        public void ClearAll()
        {
            ClearDatabase();

            using (var context = new SystemDBContext())
            {
                var felixHash = GetFelixHashId();

                // Delete all Users
                foreach (var id in context.Users.Where(x => x.HashId != felixHash).Select(e => e.Id))
                {
                    var entity = new UserModel() {Id = id};
                    context.Users.Attach(entity);
                    context.Users.Remove(entity);
                }

                // Delete all Profiles
                foreach (var id in context.Profiles.Where(x => x.UserHashId != felixHash).Select(e => e.Id))
                {
                    var entity = new ProfileModel() {Id = id};
                    context.Profiles.Attach(entity);
                    context.Profiles.Remove(entity);
                }

                context.SaveChanges();
            }
        }


        private string GetFelixHashId()
        {
            using (var context = new SystemDBContext())
            {
                var felixHash = "";
                var firstOrDefault = context.Users
                    .FirstOrDefault(x => x.Username.ToLower() == "felix");

                if (firstOrDefault != null)
                    felixHash = firstOrDefault.HashId;

                return felixHash;
            }
        }
    }
}
