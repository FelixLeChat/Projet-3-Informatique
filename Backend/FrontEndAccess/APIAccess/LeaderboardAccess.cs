using System;
using System.Collections.Generic;
using System.Net;
using Helper.Http;
using Models.Database;
using Newtonsoft.Json;

namespace FrontEndAccess.APIAccess
{
    public class LeaderboardAccess
    {
        public static LeaderboardAccess Instance;
        public string Endpoint;

        // Set endpoint to spedified value
        public LeaderboardAccess(string endpoint)
        {
            Instance = this;
            Endpoint = endpoint + "/api/leaderboard";
        }

        /// <summary>
        /// Get the leaderboard information on the given zone hash id
        /// </summary>
        /// <param name="zoneHashId"></param>
        /// <returns></returns>
        public LeaderboardModel GetLeaderboard(string zoneHashId)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/" + zoneHashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<LeaderboardModel>(body);
        }

        /// <summary>
        /// Add an entry to the leaderboard of the specified zone hash id
        /// </summary>
        /// <param name="zoneHashId"></param>
        /// <param name="leader"></param>
        public void AddLeaderEntry(string zoneHashId, LeaderModel leader)
        {
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/" + zoneHashId, leader, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }
    }
}
