namespace Models.Database
{
    public class UserModel
    {
        public int Id { get; set; }
        public string HashId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FacebookId { get; set; }
        public bool PushNotification { get; set; } = false;
        public bool FacebookPost { get; set; } = false;
    }
}