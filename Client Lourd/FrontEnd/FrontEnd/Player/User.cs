namespace FrontEnd.Player
{
    public class User
    {
        public enum LoginType
        {
            Facebook,
            Application,
            None
        }

        private static User _instance;
        public static User Instance
        {
            get { return _instance ?? (_instance = new User()); }
        }

        public bool IsConnected { get; set; }
        public string Name { get; set; }
        public string FacebookToken { get; set; }
        public string FacebookId { get; set; }
        public string UserToken { get; set; }
        public LoginType PlayerLoginType{get;set;} = LoginType.None;

        private User()
        {}

        public void Reset()
        {
            IsConnected = false;
            Name = string.Empty;
            FacebookToken = string.Empty;
            FacebookId = string.Empty;
            UserToken = string.Empty;
            PlayerLoginType = LoginType.None;
        }
    }
}
