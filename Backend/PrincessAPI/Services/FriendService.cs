using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Helper.Http;
using Microsoft.Ajax.Utilities;
using Models.Database;
using Models.Token;
using Newtonsoft.Json;
using PrincessAPI.Infrastructure;

namespace PrincessAPI.Services
{
    public class FriendService : AbstractService
    {
        public FriendService(UserToken userToken) : base(userToken)
        {
        }

        public List<BasicUserInfo> GetAllFriends()
        {
            if (UserToken == null)
                return new List<BasicUserInfo>();

            using (var db = new SystemDBContext())
            {
                // Player we added
                var userFriend = db.Friends.Where(x => x.Player1HashId == UserToken.UserId).ToList();
                var friends = userFriend.Select(friendModel => new BasicUserInfo()
                {
                    AreFriend = true,
                    HashId = friendModel.Player2HashId,
                    Username = friendModel.Player2Username
                }).ToList();

                // Players who added us
                userFriend = db.Friends.Where(x => x.Player2HashId == UserToken.UserId).ToList();
                friends.AddRange(userFriend.Select(friendModel => new BasicUserInfo()
                {
                    AreFriend = true,
                    HashId = friendModel.Player1HashId,
                    Username = friendModel.Player1Username
                }));
                
                return friends;
            }
        }

        public void UpdateFriendList(List<BasicUserInfo> friends)
        {
            if (friends == null)
                throw HttpResponseExceptionHelper.Create("Null friend list specified", HttpStatusCode.BadRequest);

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            // Delete all friends
            DeleteAllFriends();

            using (var db = new SystemDBContext())
            {
                foreach (var basicUserInfo in friends)
                {
                    if (!string.IsNullOrWhiteSpace(basicUserInfo.Username) &&
                        !string.IsNullOrWhiteSpace(basicUserInfo.HashId) &&
                        UserToken.UserId != basicUserInfo.HashId)
                    {
                        db.Friends.Add(new FriendModel()
                        {
                            Player1HashId = UserToken.UserId,
                            Player1Username = UserToken.Username,
                            Player2HashId = basicUserInfo.HashId,
                            Player2Username = basicUserInfo.Username
                        });
                    }
                }
                db.SaveChanges();
                //TrySaveDb(db);
            }
        }

        public void AddFriend(string HashId, string Username)
        {
            if (HashId.IsNullOrWhiteSpace() || Username.IsNullOrWhiteSpace())
                throw HttpResponseExceptionHelper.Create("Null friend name or id specified", HttpStatusCode.BadRequest);

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                db.Friends.Add(new FriendModel()
                {
                    Player1HashId = UserToken.UserId,
                    Player1Username = UserToken.Username,
                    Player2HashId = HashId,
                    Player2Username = Username
                });
                db.SaveChanges();
            }
        }

        public void RemoveFriend(string HashId, string Username)
        {
            if (HashId.IsNullOrWhiteSpace() || Username.IsNullOrWhiteSpace())
                throw HttpResponseExceptionHelper.Create("Null friend name or id specified", HttpStatusCode.BadRequest);

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var toremove = db.Friends.FirstOrDefault(x => x.Player1HashId == UserToken.UserId
                                               && x.Player1Username == UserToken.Username
                                               && x.Player2HashId == HashId
                                               && x.Player2Username == Username);
                if (toremove == null)
                {
                    toremove = db.Friends.FirstOrDefault(x => x.Player1HashId == HashId
                                               && x.Player1Username == Username
                                               && x.Player2HashId == UserToken.UserId
                                               && x.Player2Username == UserToken.Username);
                }
                if (toremove != null)
                {
                    db.Friends.Remove(toremove);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete all friends
        /// </summary>
        public void DeleteAllFriends()
        {

            if (UserToken == null)
                throw HttpResponseExceptionHelper.Create("User token not specified", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var userFriend =
                    db.Friends.Where(x => x.Player1HashId == UserToken.UserId || x.Player2HashId == UserToken.UserId)
                        .ToList();

                if (userFriend != null && userFriend.Count > 0)
                {
                    db.Friends.RemoveRange(userFriend);
                    db.SaveChanges();
                }
            }
        }
    }
}