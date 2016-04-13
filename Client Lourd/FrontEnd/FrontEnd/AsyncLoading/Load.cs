using System.Threading;
using System.Threading.Tasks;
using FrontEnd.Achievements;
using FrontEnd.Game.Handler;
using FrontEnd.Helper;
using FrontEnd.Model.ViewModel;
using FrontEnd.Player;
using FrontEnd.Stats;
using FrontEnd.UserControl;
using FrontEnd.UserControl.Popup;
using FrontEnd.WebCommunication;
using FrontEndAccess;
using FrontEndAccess.APIAccess;
using FrontEndAccess.WebsocketAccess;
using Models.Database;

namespace FrontEnd.AsyncLoading
{
    public class Load
    {
        public static void LoadOnLogin()
        {
            // load profile async on login
            //var profileTask = new Task(() =>
            //{
                // set profile (will set stats and achievements)
                Profile.Instance.CurrentProfile = ProfileAccess.Instance.GetUserProfile();
                AchievementManager.AchieveLogin();

                ProgressManager.InitializeRank();
                StatsManager.UpdateAchievementsUnlocked();

                // Set dayly
                Profile.Instance.Daily = DailyAccess.Instance.GetDaily();
                //DailyManager.DailyInitialize();
            //});
            //profileTask.Start();

            // Open websocket for chat
            WebsocketChatAccess.Instance.OpenSocket();

            // Open socket for connexion
            ConnexionWebsocketAccess.Instance.OpenSocket(UserToken.Token);

            // create new general chat canal
            WebsocketChatAccess.Instance.CreateNewCanal(ChatMenu.DefaultCanal, UserToken.Token);

            // Synchronize the zones
            ZoneSynchronizer.SynchronizeZone();

            // Open websocket for session event
            SessionEventWebsocketAccess.Instance.OpenSocket();

            // Todo: change to init method
            OnlineEventSubscriber.Initiate();

            ConnectionHelper.StartCheckConnectionThread();

            GameModel possibleJoin;
            if (ReconnectHelper.CanJoinBackGame(out possibleJoin))
            {
                var reconnectContext = new ReconnectViewModel(possibleJoin);
                var popup = new ReconnectToGame(reconnectContext);
                popup.ShowDialog();
            }

            GeneralOnlineEventHandler.Initiate();
        }

        public static void LoadOnLogout()
        {
            // send profile update to server
            var profile = Profile.Instance.CurrentProfile;
            ProfileAccess.Instance.UpdateUserProfile(profile);

            Disconnect();
        }

        public static void Disconnect()
        {
            // Reset user information
            User.Instance.Reset();

            // Reset profile info
            //Profile.Instance.CurrentProfile = null;

            // Close chat if it is in another window
            MainWindow.Instance.HandleChatOnLogout();

            // close websocket for chat
            WebsocketChatAccess.Instance.CloseSocket();

            // Close Websocket for the session event
            SessionEventWebsocketAccess.Instance.CloseSocket();

            // Close Socket for the connexion
            ConnexionWebsocketAccess.Instance.CloseSocket();

            // Reinitialize Stats and Achievements
            StatsManager.LogoutReinitialize();
            AchievementManager.LogoutReinitialize();

            ConnectionHelper.StopCheckConnectionThread();

            GeneralOnlineEventHandler.Stop();
        }
    }
}
