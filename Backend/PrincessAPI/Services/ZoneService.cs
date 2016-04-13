using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Helper.Http;
using Helper.Image;
using HttpHelper.Hash;
using HttpHelper.Time;
using Models.Database;
using Models.Token;
using PrincessAPI.Infrastructure;
using static Models.Database.MapModelHelper;

namespace PrincessAPI.Services
{
    public class ZoneService : AbstractService
    {
        public ZoneService(UserToken userToken) : base(userToken)
        {
        }

        /// <summary>
        /// Get the basic information of all the connected user zones
        /// </summary>
        /// <returns></returns>
        public List<MapModel> GetMyZones()
        {
            return GetUserMaps(UserToken.UserId);
        }

        /// <summary>
        /// Get all the maps of your friends
        /// </summary>
        /// <returns></returns>
        public List<MapModel> GetMyFriendsZones()
        {
            var friendService = new FriendService(UserToken);
            var friends = friendService.GetAllFriends();

            var maps = new List<MapModel>();
            foreach (var friend in friends)
            {
                maps.AddRange(GetUserMaps(friend.HashId));
            }

            return maps;
        }

        /// <summary>
        /// Get all the public maps
        /// </summary>
        /// <returns></returns>
        public List<MapModel> GetPublicZones()
        {
            using (var db = new SystemDBContext())
            {
                // public profile + friends
                var publicProfiles = new ProfileService(UserToken).GetAllPublicProfile();
                var hashIds = publicProfiles.Select(x => x.HashId).ToList();

                // maps of me, my friends or public profiles
                var maps = db.Maps.Where(map => hashIds.Contains(map.CreatorhashId) || map.CreatorhashId == UserToken.UserId).ToList();

                foreach (var map in maps)
                {
                    map.Content = "";
                }

                return maps;
            }
        }

        /// <summary>
        /// Only map who are created by public profiles
        /// </summary>
        /// <returns></returns>
        public List<MapModel> GetAllPublicZones()
        {
            using (var db = new SystemDBContext())
            {
                var publicProfilesHash = db.Profiles.Where(x => x.IsPrivate == false).Select(X=> X.UserHashId).ToList();
                var maps = db.Maps.Where(map => publicProfilesHash.Contains(map.CreatorhashId)).ToList();

                foreach (var map in maps)
                {
                    map.Content = "";
                }

                return maps;
            }
        }

        /// <summary>
        /// Get all the info related to a specified map hash id
        /// </summary>
        /// <param name="mapHashId"></param>
        /// <returns></returns>
        public MapModel GetMap(string mapHashId)
        {
            using (var db = new SystemDBContext())
            {
                var map = db.Maps.FirstOrDefault(x => x.HashId == mapHashId);

                if (map == null)
                    throw HttpResponseExceptionHelper.Create("No map correspond to the given hashId",
                        HttpStatusCode.BadRequest);

                return map;
            }
        }

        /// <summary>
        /// Get the basic information of all the specified user maps
        /// </summary>
        /// <returns></returns>
        public List<MapModel> GetUserMaps(string userId)
        {
            using (var db = new SystemDBContext())
            {
                var maps = db.Maps.Where(x => x.CreatorhashId == userId).ToList();

                foreach (var map in maps)
                {
                    map.Content = "";
                }

                return maps;
            }
        }

        /// <summary>
        /// Update map information
        /// </summary>
        /// <param name="mapModel"></param>
        /// <returns></returns>
        public void UpdateMap(MapModel mapModel)
        {
            if (string.IsNullOrWhiteSpace(mapModel.HashId))
                throw HttpResponseExceptionHelper.Create("No map hash id specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var map = db.Maps.FirstOrDefault(x => x.HashId == mapModel.HashId);

                if (map == null)
                    throw HttpResponseExceptionHelper.Create("No map correspond to the given hashId",
                        HttpStatusCode.BadRequest);

                // Update info
                if (!string.IsNullOrWhiteSpace(mapModel.Content))
                    map.Content = mapModel.Content;
                map.Level = mapModel.Level;

                map.UpdateTime = TimeHelper.CurrentCanadaTime();

                // Mark entity as modified
                db.Entry(map).State = System.Data.Entity.EntityState.Modified;
                // call SaveChanges
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Create a new map in database and return it's hashid
        /// </summary>
        /// <param name="mapModel"></param>
        /// <returns></returns>
        public string CreateNewMap(MapModel mapModel)
        {
            if (!IsValid(mapModel))
                throw HttpResponseExceptionHelper.Create("Invalid map model information", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                // set hash
                mapModel.CreatorhashId = UserToken.UserId;
                mapModel.HashId = GetMapHash(mapModel);

                if (db.Maps.FirstOrDefault(x => x.HashId == mapModel.HashId) != null)
                    throw HttpResponseExceptionHelper.Create(
                        "Map already exist with name : " + mapModel.Name + "  From user : " + UserToken.Username,
                        HttpStatusCode.BadRequest);

                mapModel.CreationDate = TimeHelper.CurrentCanadaTimeString();
                mapModel.UpdateTime = TimeHelper.CurrentCanadaTime();

                // save database
                db.Maps.Add(mapModel);
                db.SaveChanges();

                // create leaderboard for map
                var leaderboard = new LeaderboardModel()
                {
                    ZoneHashId = mapModel.HashId,
                    Leaders = LeaderboardModelHelper.GetLeaderString(new List<LeaderModel>())
                };

                db.Leaderboards.Add(leaderboard);
                db.SaveChanges();

                return mapModel.HashId;
            }
        }

        /// <summary>
        /// Add the image to server
        /// </summary>
        /// <param name="mapHashId"></param>
        /// <param name="fileData"></param>
        public void UpdateMapImage(string mapHashId, byte[] fileData)
        {
            if (string.IsNullOrWhiteSpace(mapHashId))
                throw HttpResponseExceptionHelper.Create("No Map Hash id specified", HttpStatusCode.BadRequest);

            if (fileData == null)
                throw HttpResponseExceptionHelper.Create("No image providen (null)", HttpStatusCode.BadRequest);

            using (var image = Image.FromStream(new MemoryStream(fileData)))
            {
                image.Save("c:\\tmp\\" + mapHashId + ".png", ImageFormat.Png);
            }
        }

        /// <summary>
        /// Get the byte content 
        /// </summary>
        /// <param name="mapHashId"></param>
        /// <returns></returns>
        public byte[] GetMapImage(string mapHashId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mapHashId))
                    throw HttpResponseExceptionHelper.Create("Empty map Hash Id given", HttpStatusCode.BadRequest);

                var filepath = "c:\\tmp\\" + mapHashId + ".png";
                if (!File.Exists(filepath))
                    throw HttpResponseExceptionHelper.Create("No image exist for this map", HttpStatusCode.BadRequest);

                var img = Image.FromFile(filepath);
                return ImageHelper.ImgToByteArray(img);
            }
            catch(Exception e)
            {
                throw HttpResponseExceptionHelper.Create(e.Message, HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Delete the specified map
        /// </summary>
        /// <param name="mapHashId"></param>
        public void DeleteMap(string mapHashId)
        {
            if (string.IsNullOrWhiteSpace(mapHashId))
                throw HttpResponseExceptionHelper.Create("Invalid map hash id provided", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var map = db.Maps.FirstOrDefault(x => x.HashId == mapHashId && x.CreatorhashId == UserToken.UserId);
                if (map == null)
                    throw HttpResponseExceptionHelper.Create("No zone correspond to given hash id",
                        HttpStatusCode.BadRequest);

                db.Maps.Remove(map);
                db.SaveChanges();

                var leaderboard = db.Leaderboards.FirstOrDefault(x => x.ZoneHashId == mapHashId);
                if (leaderboard != null)
                {
                    db.Leaderboards.Remove(leaderboard);
                    db.SaveChanges();
                }
            }
        }

        #region Private
        private static string GetMapHash(MapModel model)
        {
            return Sha1Hash.GetSha1HashData(model.Name + model.CreatorhashId);
        }
        #endregion
    }
}