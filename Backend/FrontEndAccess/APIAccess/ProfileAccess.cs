using System;
using System.Collections.Generic;
using System.Net;
using Helper.Http;
using Models.Database;
using Newtonsoft.Json;

namespace FrontEndAccess.APIAccess
{
    public class ProfileAccess
    {
        public static ProfileAccess Instance;
        public string Endpoint;

        // Set endpoint to spedified value
        public ProfileAccess(string endpoint)
        {
            Instance = this;
            Endpoint = endpoint + "/api/profile";
        }

        /// <summary>
        /// Get Profile model representing current logged in user profile
        /// </summary>
        /// <returns></returns>
        public ProfileModel GetUserProfile()
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<ProfileModel>(body);
        }

        /// <summary>
        /// Update current user profile with providen information
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public ProfileModel UpdateUserProfile(ProfileModel profile)
        {
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint, profile, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<ProfileModel>(body);
        }

        /// <summary>
        /// get if the user is currently online
        /// </summary>
        /// <param name="userHashId"></param>
        /// <returns></returns>
        public bool GetIsConnected(string userHashId)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/isonline/" + userHashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<bool>(body);
        }

        /// <summary>
        /// Get the information from the user
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public ProfileModel GetUserInfo(string hashId)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/user/" + hashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<ProfileModel>(body);
        }

        /// <summary>
        /// Get the info on the user wven if he is private
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public ProfileModel GetUserInfoPlease(string hashId)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/pretty/please/" + hashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<ProfileModel>(body);
        }

        /// <summary>
        /// Get all friends user and public profile user information
        /// </summary>
        /// <returns></returns>
        public List<BasicUserInfo> GetAllPublicUserInfos()
        {
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/all", null, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<List<BasicUserInfo>>(body);
        }

        /// <summary>
        /// set the visibility of the tutorial forthe specified user token
        /// </summary>
        /// <param name="visibility"></param>
        public void SetEditorTutorialVisibility(bool visibility)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/tutorial/editor/" + visibility, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode!= HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        /// <summary>
        /// set the visibility of the tutorial forthe specified user token
        /// </summary>
        /// <param name="visibility"></param>
        public void SetGameTutorialVisibility(bool visibility)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/tutorial/game/" + visibility, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        /// <summary>
        /// set the visibility of the tutorial forthe specified user token
        /// </summary>
        /// <param name="visibility"></param>
        public void SetLightTutorialVisibility(bool visibility)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/tutorial/light/" + visibility, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }
    }
}
