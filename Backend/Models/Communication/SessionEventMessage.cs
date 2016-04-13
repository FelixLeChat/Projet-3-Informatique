namespace Models.Communication 
{
    public class SessionEventMessage : IMessage
    {
        public enum OnlineSessionEventType
        {
            KickPlayerEvent = 0,
            PlayerReadyEvent,
            StartOnlineGameEvent,
            TerminateOnlineSessionEvent,
            EndOnlineGameEvent,
            PlayerForfeitEvent,
            InvitationEvent,
            MatchMakingSearchingEvent,
            MatchMakingSyncEvent,
            MatchMakingGameInvitation
        }


        public string UserToken { get; set; }
        public string OnlineSessionId { get; set; }
        public int EventType { get; set; }
        public string JsonEvent { get; set; }
        public bool Broadcast { get; set; }


        public bool IsValid()
        {
            // Check for user token validity
            if (string.IsNullOrWhiteSpace(UserToken))
                return false;

            // Check for user token validity (the MatchMaking does not have session id)
            if (OnlineSessionId == null)
                return false;

            // Check JsonEvent validity
            if (string.IsNullOrWhiteSpace(JsonEvent))
                return false;

            return true;
        }
    }
}
