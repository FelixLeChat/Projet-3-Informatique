using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using Helper.Http;
using Models.Database;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;

namespace FrontEndAccess.APIAccess
{
    public class ZoneAccess
    {
        public static ZoneAccess Instance;
        public string Endpoint;

        // Set endpoint to spedified value
        public ZoneAccess(string endpoint)
        {
            Instance = this;
            Endpoint = endpoint + "/api/zones";
        }

        /// <summary>
        /// Create a new map with the given information
        /// return the new game hash id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public string CreateMap(string name, string content, int level = 0)
        {
            var map = new MapModel()
            {
                Name = name,
                Content = content,
                Level = level
            };
            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint+"/new", map, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<string>(body);
        }

        /// <summary>
        /// Update an existing game
        /// Con only update the content and the level
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="content"></param>
        /// <param name="level"></param>
        public void UpdateMap(string hashId, string content, int level)
        {
            var map = new MapModel()
            {
                HashId = hashId,
                Content = content,
                Level = level
            };

            var httpResponse = HttpRequestHelper.PostObjectAsync(Endpoint + "/update", map, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }


        /// <summary>
        /// Add a image for the map
        /// </summary>
        /// <param name="mapHashId"></param>
        /// <param name="imageBytes"></param>
        public void UpdateMapImage(string mapHashId, byte[] imageBytes)
        {
            var httpResponse = HttpRequestHelper.PostBodyContentAsync(Endpoint + "/image/" + mapHashId, imageBytes, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        /// <summary>
        /// Get the image for the map and return it's path
        /// </summary>
        /// <param name="mapHashId"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public string GetMapImage(string mapHashId, string imagePath = null )
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/image/" + mapHashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            var fileData = JsonConvert.DeserializeObject<byte[]>(body);

            
            var path = imagePath ?? Path.GetTempPath() + mapHashId + ".png";
            using (var image = Image.FromStream(new MemoryStream(fileData)))
            {
                image.Save(path, ImageFormat.Png);
            }

            return path;
        }

        /// <summary>
        /// Delete the map with the specified hash id
        /// </summary>
        /// <param name="mapHashId"></param>
        public void DeleteMap(string mapHashId)
        {
            var httpResponse = HttpRequestHelper.DeleteAsync(Endpoint + "/" + mapHashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NoContent)
                throw new Exception(body);
        }

        /// <summary>
        /// Get all maps created by the user, they do not contain map file as it can be to big for this listing
        /// </summary>
        /// <returns></returns>
        public List<MapModel> GetMyZones()
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint+"/all", UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<List<MapModel>>(body);
        }

        /// <summary>
        /// Get all maps created by the user's friends, they do not contain map file as it can be to big for this listing
        /// </summary>
        /// <returns></returns>
        public List<MapModel> GetMyFriendsZones()
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/friends", UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<List<MapModel>>(body);
        }

        /// <summary>
        /// Get all maps created by public users, they do not contain map file as it can be to big for this listing
        /// </summary>
        /// <returns></returns>
        public List<MapModel> GetPublicZones()
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/public", UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<List<MapModel>>(body);
        }

        /// <summary>
        /// Get all the information on a map (with content of map)
        /// </summary>
        /// <param name="mapHashId"></param>
        /// <returns></returns>
        public MapModel GetMapFromId(string mapHashId)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/search/" + mapHashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<MapModel>(body);
        }

        /// <summary>
        /// Get information on all the maps of a specific user
        /// </summary>
        /// <param name="playerHashId"></param>
        /// <returns></returns>
        public List<MapModel> GetPlayerZones(string playerHashId)
        {
            var httpResponse = HttpRequestHelper.GetAsync(Endpoint + "/user/" + playerHashId, UserToken.Token).Result;
            var body = HttpRequestHelper.GetContent(httpResponse).Result;
            var statusCode = HttpRequestHelper.GetStatusCode(httpResponse);

            if (statusCode != HttpStatusCode.OK)
                throw new Exception(body);

            return JsonConvert.DeserializeObject<List<MapModel>>(body);
        }

    }
}
