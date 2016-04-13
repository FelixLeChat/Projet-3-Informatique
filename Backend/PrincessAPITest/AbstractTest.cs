using FrontEndAccess.APIAccess;
using FrontEndAccess.Ping;
using FrontEndAccess.WebsocketAccess;
using Models.Database;

namespace PrincessAPITest
{
    public abstract class AbstractTest
    {
        protected const string Endpoint = "http://ec2-52-90-46-132.compute-1.amazonaws.com";
        protected const string WsSessionEventEndpoint = "ws://ec2-52-90-46-132.compute-1.amazonaws.com/Websocket/WsSessionEventHandler.ashx";
        protected UserAccess UserAccess { get; set; }
        protected ProfileAccess ProfileAccess { get; set; }
        protected UserModel DefaultUser { get; set; }
        protected FriendAccess FriendAccess { get; set; }
        protected GameAccess GameAccess { get; set; }
        protected ZoneAccess ZoneAccess { get; set; }
        protected PingAccess PingAccess { get; set; }
        protected LeaderboardAccess LeaderboardAccess { get; set; }
        protected DailyAccess DailyAccess { get; set; }

        protected AbstractTest()
        {
            // Set default User
            DefaultUser = new UserModel()
            {
                Username = "asds98d8",
                Password = "s9d91dmadlfewpq",
                FacebookId = "asddpd0w001001e01iewiwm01mwx0w1e"
            };

            // Set UserAccess endpoint
            UserAccess = new UserAccess(Endpoint);
            ProfileAccess = new ProfileAccess(Endpoint);
            FriendAccess = new FriendAccess(Endpoint);
            GameAccess = new GameAccess(Endpoint);
            ZoneAccess = new ZoneAccess(Endpoint);
            PingAccess = new PingAccess(Endpoint);
            LeaderboardAccess = new LeaderboardAccess(Endpoint);
            DailyAccess = new DailyAccess(Endpoint);


            SessionEventWebsocketAccess.Initialize(WsSessionEventEndpoint);
        }
    }
}
