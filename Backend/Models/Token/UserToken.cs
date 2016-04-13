using System;

namespace Models.Token
{
    public class UserToken
    {
        public string Token { get; set; } = "";
        public string Username { get; set; } = "";
        public string UserId { get; set; } = "";
        public DateTime ExpirationDate { get; set; } = new DateTime();
    }
}