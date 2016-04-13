using System.Web.Http;
using Models.Communication;
using PrincessAPI.Controllers.SecureControllerHelper;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private static ConnexionService _connexionService;
        public UserController()
        {
            _connexionService = new ConnexionService();
        }

        /// <summary>
        /// Register user with given information so they can access to online features
        /// </summary>
        /// <param name="register">Register informations on user</param>
        /// <returns>User Token</returns>
        [HttpPost]
        [Route("register")]
        public string Register(RegisterMessage register)
        {
            return _connexionService.Register(register);
        }

        /// <summary>
        /// Login function to obtain the token needed to make other requests
        /// </summary>
        /// <param name="login">Login information for Facebook or regular login</param>
        /// <returns>User Token</returns>
        [HttpPost]
        [Route("Login")]
        public string Login(LoginMessage login)
        {
            return _connexionService.Login(login);
        }

        /// <summary>
        /// Delete the user with the given User Token form the Database (you can only delete yourself)
        /// </summary>
        [HttpDelete]
        [SecureAPI]
        public void DeleteUser()
        {
            _connexionService.DeleteUser(SecureController.GetUserToken());
        }
    }
}