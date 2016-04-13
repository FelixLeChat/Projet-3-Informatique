using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Helper.Http
{
    public static class HttpResponseExceptionHelper
    {

        public static HttpResponseException Create(string reason, HttpStatusCode code, string content = null)
        {
            var msg = new HttpResponseMessage(code)
            {
                ReasonPhrase = reason,
                Content = content == null ? new StringContent(reason) : new StringContent(content)
            };

            return new HttpResponseException(msg);
        }
    }
}
