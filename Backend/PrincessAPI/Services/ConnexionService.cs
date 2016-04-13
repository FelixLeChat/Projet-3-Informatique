using System.Linq;
using System.Net;
using Helper.Hash;
using Helper.Http;
using Helper.Jwt;
using HttpHelper.Hash;
using Models;
using Models.Communication;
using Models.Database;
using Models.Token;
using PrincessAPI.Infrastructure;

namespace PrincessAPI.Services
{
    public class ConnexionService
    {
        public string Login(LoginMessage message)
        {
            if (!message.IsValid())
                throw HttpResponseExceptionHelper.Create("Information manquante pour authentification.", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                var user = new UserModel();
                var facebookLogin = false;

                // login with facebook
                if (!string.IsNullOrWhiteSpace(message.FacebookId))
                {
                    user = db.Users.FirstOrDefault(x => x.FacebookId == message.FacebookId);
                    facebookLogin = true;
                }
                // login with Credentials
                else
                {
                    user = db.Users.FirstOrDefault(x => x.Username == message.Username);
                }

                if (user == null)
                    throw HttpResponseExceptionHelper.Create("usager invalide.", HttpStatusCode.BadRequest);

                // Connect with user credentials
                if (!facebookLogin)
                    if (!PasswordHash.ValidatePassword(message.Password, user.Password))
                        throw HttpResponseExceptionHelper.Create("Information invalides pour la connexion.", HttpStatusCode.Forbidden);
                
                var hashId = user.HashId;
                //if(ConnexionWebsocket.ConnectedUsersHash.Contains(hashId))
                    //throw HttpResponseExceptionHelper.Create("L'Usager est déjà connecter.", HttpStatusCode.Forbidden);
                return GetToken(user);
            }
        }

        public string Register(RegisterMessage register)
        {
            // check length
            if (!string.IsNullOrWhiteSpace(register.Username))
            {
                if (register.Username.Length > 20)
                    throw HttpResponseExceptionHelper.Create("Votre nom d'usager excède 20 caractères",
                        HttpStatusCode.BadRequest);
                if (register.Username.Contains(" "))
                    throw HttpResponseExceptionHelper.Create("Votre nom d'usager ne doit pas contenir d'espace",
                        HttpStatusCode.BadRequest);
            }
            UserModel newUser;

            //Check if credentials are OK
            if (string.IsNullOrWhiteSpace(register.Username))
                throw HttpResponseExceptionHelper.Create("Aucun nom d'usager spécifier", HttpStatusCode.BadRequest);

            if (!register.Username.All(char.IsLetterOrDigit))
                throw HttpResponseExceptionHelper.Create("Le nom d'usager doit être alpla numérique", HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(register.Password) && string.IsNullOrWhiteSpace(register.FacebookId))
                throw HttpResponseExceptionHelper.Create("Le format d'enregistrement est invalide", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                UserModel user = null;

                // Search facebook user and then username
                if (!string.IsNullOrWhiteSpace(register.FacebookId))
                    user = db.Users.FirstOrDefault(x => x.FacebookId == register.FacebookId);
                if (user != null)
                    throw HttpResponseExceptionHelper.Create("L'usager Facebook existe déjà", HttpStatusCode.BadRequest);

                if (!string.IsNullOrWhiteSpace(register.Username))
                    user = db.Users.FirstOrDefault(x => x.Username == register.Username);
                // User already exist
                if (user != null)
                    throw HttpResponseExceptionHelper.Create("Un usager avec ce nom existe déjà",
                        HttpStatusCode.BadRequest);

                // create new user
                newUser = new UserModel()
                {
                    FacebookId = register.FacebookId,
                    Username = register.Username,
                    Password = PasswordHash.CreateHash(register.Password),
                    FacebookPost = false,
                    PushNotification = false
                };
                newUser.HashId = ModelHash.GetUserHash(newUser);

                var newProfile = ProfileModelHelper.Initialize(new ProfileModel());
                newProfile.UserHashId = newUser.HashId;
                newProfile.Username = register.Username;


                // ALL MIGHTY POWER
                // HACK FOR FELIX USER
                if (newUser.Username.ToLower().Equals("felix"))
                {
                    newProfile.Experience = 9000;
                    newProfile.PrincessTitle = EnumsModel.PrincessTitle.MASTER;
                    newProfile.Description = "ALL MIGHTY GOD OF THE BACKEND AND BACKLOG";
                    newProfile.Picture = EnumsModel.PrincessAvatar.Mulan;
                    newProfile.AchievementsString =
                        "{\"AddAvatar\":true,\"FastGamePoints\":true,\"FinishCampain\":true,\"FinishOtherSucess\":true,\"FirstMapCreated\":true,\"FirstOnlineGame\":true,\"FirstOnlineGameWon\":true,\"FirstTimeConnect\":true,\"GamePoints\":true,\"PlayWithAFriend\":true}";
                }

                // Add user
                newUser = CreateUser(newUser);

                // Add profile
                db.Profiles.Add(newProfile);
                db.SaveChanges();
            }

            return GetToken(newUser);
        }

        public void DeleteUser(UserToken userToken)
        {
            using (var db = new SystemDBContext())
            {
                var user = db.Users.FirstOrDefault(x => x.HashId == userToken.UserId);
                if (user == null)
                    throw HttpResponseExceptionHelper.Create("Identifiant d'usager invalide", HttpStatusCode.BadRequest);

                db.Users.Remove(user);
                db.SaveChanges();
            }

            // delete all users friends
            var friendService = new FriendService(userToken);
            friendService.DeleteAllFriends();

            // delete profile
            var profileService = new ProfileService(userToken);
            profileService.DeleteMyProfile();

            // Delete daylies
            var dailyService = new DailyService(userToken);
            dailyService.DeleteAllDaily();
        }

        #region Private
        private UserModel CreateUser(UserModel user)
        {
            //Check if user is valid
            if (string.IsNullOrWhiteSpace(user.Username) && string.IsNullOrWhiteSpace(user.FacebookId))
                throw HttpResponseExceptionHelper.Create("Informations invalides", HttpStatusCode.BadRequest);

            using (var db = new SystemDBContext())
            {
                // save database
                var newUser = db.Users.Add(user);
                db.SaveChanges();
                return newUser;
            }
        }

        private string GetToken(UserModel user)
        {
            // Generate token
            var token = new UserToken()
            {
                Username = user.Username,
                UserId = user.HashId
            };
            return JwtHelper.EncodeToken(token);
        }
        #endregion

    }
}