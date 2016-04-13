namespace Models
{
    public static class EnumsModel
    {
        public enum PrincessTitle
        {
            Lady,
            Duchess,
            Princess,
            Queen,
            MASTER
        }

        public static string ToString(this PrincessTitle title)
        {
            var result = "";

            switch (title)
            {
                case PrincessTitle.Lady:
                    result = "Lady";
                    break;
                case PrincessTitle.Duchess:
                    result = "Duchess";
                    break;
                case PrincessTitle.Princess:
                    result = "Princess";
                    break;
                case PrincessTitle.Queen:
                    result = "Queen";
                    break;
                default:
                    result = "Invalid Title";
                    break;
            }
            return result;
        }

        public enum GameState
        {
            Waiting,
            Started,
            Ended
        }

        public enum Stats
        {
            TotalPoints,
            TotalTimePlayed,
            TotalMapCreated,
            TotalGameWon,
            TotalGamePlayed,
            SucessUnlocked
        }

        public enum Achievement
        {
            FirstTimeConnect,
            AddAvatar,
            FastGamePoints,
            GamePoints,
            FirstOnlineGame,
            FirstMapCreated,
            FirstOnlineGameWon,
            PlayWithAFriend,
            FinishCampain,
            FinishOtherSucess
        }

        public static string GetAchievementDescription(EnumsModel.Achievement achievement)
        {
            var text = "";
            switch ((int)achievement)
            {
                case 0:
                    text += "Se connecter pour la première fois dans le client lourd.";
                    break;
                case 1:
                    text += "Ajouter un avatar à son compte.";
                    break;
                case 2:
                    text += "Obtenir 10000 points ou plus dans une seule partie en mode rapide.";
                    break;
                case 3:
                    text += "Obtenir 10000 points ou plus dans tous type de parties.";
                    break;
                case 4:
                    text += "Jouer ta première partie en mode réseau.";
                    break;
                case 5:
                    text += "Créer ta première carte dans le client lourd.";
                    break;
                case 6:
                    text += "Gagner une partie en mode réseau.";
                    break;
                case 7:
                    text += "Jouer à une zone de jeu qu'un ami a créé.";
                    break;
                case 8:
                    text += "Terminer le mode campagne.";
                    break;
                case 9:
                    text += "Obtenir les 9 autres succès.";
                    break;
            }

            return text;
        }

        public static string GetAchievementPath(Achievement achievement)
        {
            var path = "";
            switch (achievement)
            {
                case Achievement.FirstTimeConnect:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Login_A.png";
                    break;
                case Achievement.AddAvatar:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Avatar_A.png";
                    break;
                case Achievement.FastGamePoints:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Fast_A.png";
                    break;
                case Achievement.GamePoints:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Game_A.png";
                    break;
                case Achievement.FirstOnlineGame:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Network_A.png";
                    break;
                case Achievement.FirstMapCreated:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Map_A.png";
                    break;
                case Achievement.FirstOnlineGameWon:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Win_A.png";
                    break;
                case Achievement.PlayWithAFriend:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Friend_A.png";
                    break;
                case Achievement.FinishCampain:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Campain_A.png";
                    break;
                case Achievement.FinishOtherSucess:
                    path = "http://ec2-52-90-46-132.compute-1.amazonaws.com/Content/Images/Achievements/Achievement_A.png";
                    break;
            }
            

            return path;
        }

        public enum PrincessAvatar
        {
            Default,
            Ariel,
            Belle,
            Cinder,
            Frog,
            Mulan,
            Poca,
            Ray,
            Sleep,
            Snow,
            Jasmine
        }

        public enum DailyType
        {
            CreateMap,
            PlayCampain,
            PlayFastGame,
            AddFriend,
            UpdateProfile,
            ChatOnce,
            CreateCanal
        }

        public enum AccessLevel
        {
            Me,
            MyFriends,
            Everyone
        }

        public enum TutorialType
        {
            Editor, Game, Light
        }
    }
}