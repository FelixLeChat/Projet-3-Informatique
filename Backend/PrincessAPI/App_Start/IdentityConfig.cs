using System.Security.Principal;
using Helper.Jwt;
using Models;
using Models.Database;
using Models.Token;

namespace PrincessAPI
{
    public class PrincessSerializable
    {
        public int Id { get; set; }
        public string UserHashId { get; set; }
        public string Username { get; set; }
        public EnumsModel.PrincessAvatar Picture { get; set; }
        public EnumsModel.PrincessTitle PrincessTitle { get; set; }

        public string Token { get; set; }
    }

    public class PrincessPrincipal : IPrincipal
    {
        private UserToken token_ = null;
        public IIdentity Identity { get; private set; }
        public PrincessSerializable Profile { get; set; }

        public  UserToken Token {
            get
            {
                if (token_ == null)
                    token_ = JwtHelper.DecodeToken(Profile.Token);
                return token_;
            } 
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public PrincessPrincipal(PrincessSerializable model)
        {
            Profile = model;
            this.Identity = new GenericIdentity(model.Username);
        }

        public int UserId { get; set; }
    }
}