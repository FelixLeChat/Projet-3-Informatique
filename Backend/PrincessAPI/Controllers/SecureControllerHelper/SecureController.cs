using System;
using System.Net;
using System.Web;
using System.Web.Http;
using Helper.Http;
using Helper.Jwt;
using Models.Token;

namespace PrincessAPI.Controllers.SecureControllerHelper
{
    /// <summary>
    /// Provide an Authentification purpose
    /// It will take the token and generate the appropriate User Credentials to be used in others
    /// Services to authentify the user who request API informations.
    /// </summary>
    public class SecureController : ApiController
    {
        protected UserToken UserToken { get; set; }
        protected SecureController()
        {
            UserToken = GetUserToken();
        }

        public static UserToken GetUserToken()
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];

                if (string.IsNullOrWhiteSpace(token))
                    throw HttpResponseExceptionHelper.Create("NULL or empty token - Secure controller", HttpStatusCode.Forbidden);

                return JwtHelper.DecodeToken(token);
            }
            catch (Exception)
            {
                throw HttpResponseExceptionHelper.Create("Invalid token - Secure controller", HttpStatusCode.Forbidden);
            }
        }
    }
}