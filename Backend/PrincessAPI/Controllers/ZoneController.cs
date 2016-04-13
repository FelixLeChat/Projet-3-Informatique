using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Http;
using Helper.Http;
using Models.Database;
using PrincessAPI.Controllers.SecureControllerHelper;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers
{
    [SecureAPI]
    [RoutePrefix("api/zones")]
    public class ZoneController : SecureController
    {
        private readonly ZoneService _zoneService;
        public ZoneController()
        {
            _zoneService = new ZoneService(UserToken);
        }

        /// <summary>
        /// Get all the zone the user created
        /// </summary>
        /// <returns>List of zone information</returns>
        [HttpGet]
        [Route("all")]
        public List<MapModel> GetMyZones()
        {
            return _zoneService.GetMyZones();
        }

        /// <summary>
        /// Get all the public zone and them only
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("allpublic")]
        public List<MapModel> GetAllPublicZones()
        {
            return _zoneService.GetAllPublicZones();
        }

        /// <summary>
        /// Get all the zones created by the user's friends
        /// </summary>
        /// <returns>List of zone information</returns>
        [HttpGet]
        [Route("friends")]
        public List<MapModel> GetMyFriendsZones()
        {
            return _zoneService.GetMyFriendsZones();
        }

        /// <summary>
        /// Get all the public zones including the private zones of friends
        /// </summary>
        /// <returns>List of zone information</returns>
        [HttpGet]
        [Route("public")]
        public List<MapModel> GetPublicZones()
        {
            return _zoneService.GetPublicZones();
        }

        /// <summary>
        /// get all the information, including the content of the spccified zone
        /// </summary>
        /// <param name="mapId">Id of the zone</param>
        /// <returns>Information on the given zone</returns>
        [HttpGet]
        [Route("search/{mapId}")]
        public MapModel GetMap(string mapId)
        {
            return _zoneService.GetMap(mapId);
        }

        /// <summary>
        /// Get the list of the zone for the given player ID
        /// </summary>
        /// <param name="userId">Id of the player</param>
        /// <returns>List of zone information</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public List<MapModel> GetUserMaps(string userId)
        {
            return _zoneService.GetUserMaps(userId);
        }

        /// <summary>
        /// Get the byte of the image related to the given zone ID
        /// </summary>
        /// <param name="mapHashId">Id of the zone</param>
        /// <returns>Image</returns>
        [HttpGet]
        [Route("image/{mapHashId}")]
        public byte[] GetMapImage(string mapHashId)
        {
            return _zoneService.GetMapImage(mapHashId);
        }

        /// <summary>
        /// Update the information on the given zone
        /// </summary>
        /// <param name="mapModel">Id of the zone</param>
        [HttpPost]
        [Route("update")]
        public void Updatemap(MapModel mapModel)
        {
            _zoneService.UpdateMap(mapModel);
        }

        /// <summary>
        /// Createa new map with the given information
        /// </summary>
        /// <param name="mapModel">Information on the zone</param>
        /// <returns>Id of the zone</returns>
        [HttpPost]
        [Route("new")]
        public string CreateMap(MapModel mapModel)
        {
            return _zoneService.CreateNewMap(mapModel);
        }

        static readonly string ServerUploadFolder = Path.GetTempPath();

        /// <summary>
        /// Set the image for the given zone
        /// </summary>
        /// <param name="mapHashId">Image</param>
        [HttpPost]
        [Route("image/{mapHashId}")]
        public void UpdateMapImage(string mapHashId)
        {
            try
            {
                byte[] fileData;
                var stream = Request.Content.ReadAsStreamAsync().Result;
                using (var binaryReader = new BinaryReader(stream))
                {
                    fileData = binaryReader.ReadBytes((int)stream.Length);
                }
                _zoneService.UpdateMapImage(mapHashId, fileData);
            }
            catch (Exception e)
            {
                throw HttpResponseExceptionHelper.Create(e.Message, HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Delete the map with the given Id
        /// </summary>
        /// <param name="mapHashId">Id of the zone</param>
        [HttpDelete]
        [Route("{mapHashId}")]
        public void DeleteMap(string mapHashId)
        {
            _zoneService.DeleteMap(mapHashId);
        }
    }
}
