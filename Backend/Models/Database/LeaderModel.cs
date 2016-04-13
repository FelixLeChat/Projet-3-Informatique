namespace Models.Database
{
    public class LeaderModel
    {
        public string PlayerHashId { get; set; }
        public string PlayerName { get; set; }
        public int Points { get; set; }
    }

    public class LeaderModelHelper
    {
        public static bool IsValid(LeaderModel leader)
        {
            if (leader == null)
                return false;
            if (string.IsNullOrWhiteSpace(leader.PlayerHashId))
                return false;
            if (string.IsNullOrWhiteSpace(leader.PlayerName))
                return false;
            if (leader.Points < 0)
                return false;

            return true;
        }
    }
}