namespace Models.Communication
{
    public class LoginMessage : IMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FacebookId { get; set; }

        public bool IsValid()
        {
            // Check if not empty credentials
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
                return true;

            // Check for not empty Facebook Credentials
            return !string.IsNullOrWhiteSpace(FacebookId);
        }
    }
}