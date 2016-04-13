using System;
using System.Collections.Generic;
using System.Net;
using Helper.Http;
using Models.Database;
using Newtonsoft.Json;

namespace FrontEndAccess.APIAccess
{
    public class FriendAccess
    {
        public static FriendAccess Instance;
        public string Endpoint;

        // Set endpoint to spedified value
        public FriendAccess(string endpoint)
        {
            Endpoint = endpoint + "/api/friend";
        }

        /// <summary>
        /// Get a list of all the user's friends
        /// </summary>
        /// <returns></returns>
        public List<BasicUserInfo> GetFriends()
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<List<BasicUserInfo>>(body);
        }

        /// <summary>
        /// Update friend list with specified friend list
        /// </summary>
        /// <param name="friends"></param>
        public void UpdateFriends(List<BasicUserInfo> friends)
        {
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint, friends, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }
    }
}
