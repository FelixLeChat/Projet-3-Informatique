using System.Collections.Generic;
using System.Web.Http;
using Models.Database;
using PrincessAPI.Controllers.SecureControllerHelper;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers
{
    [SecureAPI]
    [RoutePrefix("api/friend")]
    public class FriendController : SecureController
    {
        private readonly FriendService _friendService;
        public FriendController()
        {
            _friendService = new FriendService(UserToken);
        }

        /// <summary>
        /// Get Friend of user
        /// </summary>
        /// <returns>List of basic information on the friends of the user</returns>
        [HttpGet]
        public List<BasicUserInfo> GetFriends()
        {
            return _friendService.GetAllFriends();
        }

        /// <summary>
        /// Update the list of friend for the user with the one given
        /// </summary>
        /// <param name="friends">List of friends to update</param>
        [HttpPost]
        public void UpdateFriends(List<BasicUserInfo> friends)
        {
            _friendService.UpdateFriendList(friends);
        }

    }
}
