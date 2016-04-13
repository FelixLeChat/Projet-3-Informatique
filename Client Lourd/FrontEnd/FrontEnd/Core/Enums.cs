namespace FrontEnd.Core
{
    public class Enums
    {

        /// <summary>
        /// États possible du jeux (if add a new one, need to update the StateManager
        /// </summary>
        public enum States
        {
            MainMenu,
            GameMenu,
            OfflineGame,
            OnlineGame,
            Options,
            Login,
            Register,
            Profile,
            ProfileEdit,
            UserFriends,
            UserZones,
            Edition,
            MatchMakingSearching,
            MatchMakingHosting
        }

    }
}
