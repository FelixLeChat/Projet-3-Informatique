using System;
using System.IO;
using System.Net;
using Helper.Http;
using Models.Communication;
using Newtonsoft.Json;

namespace FrontEndAccess.APIAccess
{
    public class UserAccess
    {
        public static UserAccess Instance;
        public string Endpoint;

        // Set endpoint to spedified value
        public UserAccess(string endpoint)
        {
            Endpoint = endpoint + "/api/user";
        }

        /// <summary>
        /// Register user in database and return his token for api calls.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="facebookId"></param>
        /// <returns></returns>
        public string Register(string username, string password, string facebookId = "")
        {
            var message = new RegisterMessage()
            {
                FacebookId = facebookId,
                Username = username,
                Password = password
            };

            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/register", message).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if(statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            // setup token
            var token = JsonConvert.DeserializeObject<string>(body);
            if (!string.IsNullOrWhiteSpace(token))
                UserToken.Token = token;

            return token;
        }

        /// <summary>
        /// send credentials to get user token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="facebookId"></param>
        /// <returns></returns>
        public string Login(string username, string password, string facebookId = "")
        {
            var message = new LoginMessage()
            {
                FacebookId = facebookId,
                Username = username,
                Password = password
            };

            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/login", message).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            var token = JsonConvert.DeserializeObject<string>(body);
            if (!string.IsNullOrWhiteSpace(token))
                UserToken.Token = token;

            return token;
        }

        /// <summary>
        /// Delete current user
        /// </summary>
        public void DeleteUser()
        {
            if (UserToken.Token == "")
                return;

            var httpResponse = HttpRequestHelper.DeleteAsync(Endpoint, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        public string GetFacebookId(string facebookToken)
        {
            if (string.IsNullOrWhiteSpace(facebookToken))
                return "";

            var geturl = WebRequest.Create("https://graph.facebook.com/v2.5/me?access_token=" + facebookToken);
            var objStream = geturl.GetResponse().GetResponseStream();
            if (objStream != null)
            {
                var objReader = new StreamReader(objStream);

                dynamic data = JsonConvert.DeserializeObject(objReader.ReadLine());

                try
                {
                    return data.id;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return "";
        }
    }
}
