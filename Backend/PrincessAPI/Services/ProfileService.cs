using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using Helper.Http;
using Models.Database;
using Models.Token;
using PrincessAPI.Infrastructure;
using PrincessAPI.Websocket;

namespace PrincessAPI.Services
{
    public class ProfileService : AbstractService
    {
        public ProfileService(UserToken userToken) : base(userToken)
        {
        }

        public ProfileModel GetMyProfile()
        {
            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var profile = db.Profiles.FirstOrDefault(x => x.UserHashId == UserToken.UserId);
                if (profile == null)
                    throw HttpResponseExceptionHelper.Create("No profile exist with specified token",
                        HttpStatusCode.BadRequest);

                return profile;
            }
        }

        public ProfileModel GetUserProfile(string userId, bool checkPrivate = true)
        {
            using (var db = new SystemDBContext())
            {
                var profile = db.Profiles.FirstOrDefault(x => x.UserHashId == userId);
                if (profile == null)
                    throw HttpResponseExceptionHelper.Create("No profile exist with specified user Id",
                        HttpStatusCode.BadRequest);

                if(checkPrivate)
                    if(profile.IsPrivate)
                        throw HttpResponseExceptionHelper.Create("User profile is private",
                            HttpStatusCode.Unauthorized);

                return profile;
            }
        }

        public ProfileModel UpdateMyProfile(ProfileModel newProfile)
        {

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);
            using (var db = new SystemDBContext())
            {
                var profile = db.Profiles.FirstOrDefault(x => x.UserHashId == UserToken.UserId);
                if (profile == null)
                    throw HttpResponseExceptionHelper.Create("No profile exist with specified user Id",
                        HttpStatusCode.BadRequest);


                // Modify profile
                if (newProfile == null)
                    throw HttpResponseExceptionHelper.Create("Invalid providen profile",
                        HttpStatusCode.BadRequest);

                profile.IsPrivate = newProfile.IsPrivate;
                profile.Description = newProfile.Description;
                profile.Picture = newProfile.Picture;

                if ((int)newProfile.PrincessTitle > (int)profile.PrincessTitle)
                    profile.PrincessTitle = newProfile.PrincessTitle;

                if (newProfile.Experience > profile.Experience)
                    profile.Experience = newProfile.Experience;

                if(newProfile.Level > profile.Level)
                    profile.Level = newProfile.Level;

                if (!string.IsNullOrWhiteSpace(newProfile.StatsString))
                    profile.StatsString = newProfile.StatsString;

                if(!string.IsNullOrWhiteSpace(newProfile.AchievementsString) && !newProfile.AchievementsString.Contains("null"))
                    profile.AchievementsString = newProfile.AchievementsString;

                // call SaveChanges
                db.SaveChanges();
                return profile;
            }
        }

        public List<BasicUserInfo> GetAllPublicProfile()
        {
            using (var db = new SystemDBContext())
            {
                // get all public profiles
                var profiles = db.Profiles.Where(x => x.IsPrivate == false).ToList();
                var visibleUser = new List<BasicUserInfo>();

                // get all friends
                var friendService = new FriendService(UserToken);
                visibleUser.AddRange(friendService.GetAllFriends());

                foreach (var profile in profiles)
                {
                    // not contained in visible user yet ( not friends )
                    if (visibleUser.FirstOrDefault(x => x.HashId == profile.UserHashId) == null)
                    {
                        // search for user and add info if exist
                        var user = db.Users.FirstOrDefault(x => x.HashId == profile.UserHashId);
                        if(user != null)
                            visibleUser.Add(new BasicUserInfo()
                            {
                                AreFriend = false,
                                HashId = user.HashId,
                                Username = user.Username
                            });
                    }
                }
                return visibleUser;
            }
        }

        public void SetEditorTutorialVisibility(bool visibility)
        {

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var profile = db.Profiles.FirstOrDefault(x => x.UserHashId == UserToken.UserId);
                if (profile == null)
                    throw HttpResponseExceptionHelper.Create("No profile exist with specified user Id",
                        HttpStatusCode.BadRequest);

                profile.ShowEditorTutorial = visibility;
                db.SaveChanges();
            }
        }

        public void SetGameTutorialVisibility(bool visibility)
        {

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var profile = db.Profiles.FirstOrDefault(x => x.UserHashId == UserToken.UserId);
                if (profile == null)
                    throw HttpResponseExceptionHelper.Create("No profile exist with specified user Id",
                        HttpStatusCode.BadRequest);

                profile.ShowGameTutorial = visibility;
                db.SaveChanges();
            }
        }

        public void SetLightTutorialVisibility(bool visibility)
        {

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var profile = db.Profiles.FirstOrDefault(x => x.UserHashId == UserToken.UserId);
                if (profile == null)
                    throw HttpResponseExceptionHelper.Create("No profile exist with specified user Id",
                        HttpStatusCode.BadRequest);

                profile.ShowLightTutorial = visibility;
                db.SaveChanges();
            }
        }

        public bool GetIsOnline(string userHashid)
        {
            if (string.IsNullOrWhiteSpace(userHashid))
                throw HttpResponseExceptionHelper.Create("Pas de hash spécifier", HttpStatusCode.BadRequest);

            return ConnexionWebsocket.ConnectedUsersHash.Contains(userHashid);
        }

        public void DeleteMyProfile()
        {

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var profile = db.Profiles.FirstOrDefault(x => x.UserHashId == UserToken.UserId);
                if (profile == null)
                    throw HttpResponseExceptionHelper.Create("No profile exist with specified user Id",
                        HttpStatusCode.BadRequest);

                db.Profiles.Remove(profile);
                db.SaveChanges();
            }
        }
    }
}