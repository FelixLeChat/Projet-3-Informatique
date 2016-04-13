using System.Collections.Generic;
using System.Web.Http;
using Models.Database;
using PrincessAPI.Controllers.SecureControllerHelper;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers
{
    [SecureAPI]
    [RoutePrefix("api/profile")]
    public class ProfileController : SecureController
    {
        private readonly ProfileService _profileService;
        public ProfileController()
        {
            _profileService = new ProfileService(UserToken);
        }

        /// <summary>
        /// Get the profile of the current logged in player
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ProfileModel GetMyProfile()
        {
            return _profileService.GetMyProfile();
        }

        /// <summary>
        /// Get the public profile information on the given player
        /// </summary>
        /// <param name="userId">Id of the player</param>
        /// <returns>Public profile information</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public ProfileModel GetUserProfile(string userId)
        {
            return _profileService.GetUserProfile(userId);
        }

        /// <summary>
        /// Get is the given user is online
        /// </summary>
        /// <param name="userHashId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("isonline/{userHashId}")]
        public bool GetIsOnline(string userHashId)
        {
            return _profileService.GetIsOnline(userHashId);
        }

        /// <summary>
        /// Get all the information on the given user (even if private profile)
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <returns>Information on user</returns>
        [HttpGet]
        [Route("pretty/please/{userId}")]
        public ProfileModel GetUserProfilePlease(string userId)
        {
            return _profileService.GetUserProfile(userId, false);
        }

        /// <summary>
        /// Update the profile informaiton on the given player
        /// </summary>
        /// <param name="profile">Profile informaiton</param>
        /// <returns>Profile information</returns>
        [HttpPost]
        public ProfileModel UpdateMyProfile(ProfileModel profile)
        {
            return _profileService.UpdateMyProfile(profile);
        }

        /// <summary>
        /// Get all the information on public users
        /// </summary>
        /// <returns>List of user information</returns>
        [HttpPost]
        [Route("all")]
        public List<BasicUserInfo> GetAllPublicUsers()
        {
            return _profileService.GetAllPublicProfile();
        }

        /// <summary>
        /// Set the tutorial visibility for the Editor
        /// </summary>
        /// <param name="visibility">Visibility of the tutorial</param>
        [HttpGet]
        [Route("tutorial/editor/{visibility}")]
        public void SetEditorTutorialVisibility(bool visibility)
        {
            _profileService.SetEditorTutorialVisibility(visibility);
        }

        /// <summary>
        /// Set the tutorial visibility for the Game Tutorial
        /// </summary>
        /// <param name="visibility">Visibility of the tutorial</param>
        [HttpGet]
        [Route("tutorial/game/{visibility}")]
        public void SetGameTutorialVisibility(bool visibility)
        {
            _profileService.SetGameTutorialVisibility(visibility);
        }

        /// <summary>
        /// Set the tutorial visibility for the Light Client Tutorial
        /// </summary>
        /// <param name="visibility">Visibility of the tutorial</param>
        [HttpGet]
        [Route("tutorial/light/{visibility}")]
        public void SetLightTutorialVisibility(bool visibility)
        {
            _profileService.SetLightTutorialVisibility(visibility);
        }
    }
}
