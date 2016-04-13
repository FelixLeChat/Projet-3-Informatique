using System.Web.Http;

namespace PrincessAPI.Controllers
{
    [RoutePrefix("api/ping")]
    public class PingController : ApiController
    {
        /// <summary>
        /// Get the time to handle a request from the client to a response from the server
        /// </summary>
        [HttpGet]
        public void GetPing()
        {
        }
    }
}
