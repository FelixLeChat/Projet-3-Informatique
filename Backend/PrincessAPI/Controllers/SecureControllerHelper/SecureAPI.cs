using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Helper.Jwt;

namespace PrincessAPI.Controllers.SecureControllerHelper
{
    /// <summary>
    /// Provide Authorization purposes
    /// It will check before entering the API Endpoint if the Token is a valid one and throw
    /// a Aunothorized Http Response if the Token is invalid
    /// </summary>
    public class SecureAPI : AuthorizeAttribute
    {
        // Manage default othorization (1)
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }

        // Check if user is authorized (2)
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var token = HttpContext.Current.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(token))
                return false;

            return JwtHelper.ValidateToken(token);
        }

        // What to do if user is not Authorized (3)
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
}