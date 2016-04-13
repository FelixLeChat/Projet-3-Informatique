using System.Web.Http;
using Models.Database;
using PrincessAPI.Controllers.SecureControllerHelper;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers
{
    [SecureAPI]
    [RoutePrefix("api/daily")]
    public class DailyController : SecureController
    {
        private readonly DailyService _dailyService;
        public DailyController()
        {
            _dailyService = new DailyService(UserToken);
        }

        /// <summary>
        /// Obtain information on the Daily Challenge for the user
        /// </summary>
        /// <returns>Information on the Daily</returns>
        [HttpGet]
        public DailyModel GetDaily()
        {
            return _dailyService.GetDaily();
        }

        /// <summary>
        /// Mark the daily challenge of the user as completed
        /// </summary>
        [HttpGet]
        [Route("done")]
        public void FinishDaily()
        {
            _dailyService.CompleteDaily();
        }

        /// <summary>
        /// Delete all pass and present daily challenge for the user
        /// </summary>
        [HttpDelete]
        public void DeleteAllDaily()
        {
            _dailyService.DeleteAllDaily();
        }
    }
}
