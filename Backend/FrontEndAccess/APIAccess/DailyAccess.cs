using System;
using System.Net;
using Helper.Http;
using Models.Database;
using Newtonsoft.Json;

namespace FrontEndAccess.APIAccess
{
    public class DailyAccess
    {
        public static DailyAccess Instance;
        public string Endpoint;

        // Set endpoint to spedified value
        public DailyAccess(string endpoint)
        {
            Endpoint = endpoint + "/api/daily";
        }

        /// <summary>
        /// Get a list of all the user's friends
        /// </summary>
        /// <returns></returns>
        public DailyModel GetDaily()
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<DailyModel>(body);
        }

        /// <summary>
        /// Get a list of all the user's friends
        /// </summary>
        /// <returns></returns>
        public void CompleteDaily()
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/done", UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }
    }
}