namespace Models.Communication 
{
    public class WebsocketChatMessage : IMessage
    {
        public string Message { get; set; }
        public string CanalId { get; set; }
        public string CanalName { get; set; }
        public string UserToken { get; set; }

        public string TargetUserHashId { get; set; }
        public string TargetUserName { get; set; }

        public bool IsValid()
        {
            // Check for user token validity
            if (string.IsNullOrWhiteSpace(UserToken))
                return false;

            // Check for message in channel validity
            if (!string.IsNullOrWhiteSpace(CanalId) && !string.IsNullOrWhiteSpace(Message))
                return true;

            // Check for new channel validity
            if (!string.IsNullOrWhiteSpace(CanalName))
                return true;

            // Check for friend canal
            if (!string.IsNullOrWhiteSpace(TargetUserName) && !string.IsNullOrWhiteSpace(TargetUserHashId))
                return true;

            return false;
        }
    }
}
