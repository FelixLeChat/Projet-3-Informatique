namespace Models.Database
{
    public class FriendRequestModel
    {
        public int Id { get; set; }
        public string SenderHash { get; set; } = "";
        public string ReceiverHash { get; set; } = "";
        public bool Accepted { get; set; } = false;
    }
}