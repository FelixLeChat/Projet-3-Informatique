using System.Web.Mvc;
using Helper.Jwt;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers.Site
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var user = User as PrincessPrincipal;
            var ps = new ProfileService(null);
            if(user!=null)
            {
                ps = new ProfileService(user.Token);
            }
            return View(ps.GetAllPublicProfile());
        }
    }
}