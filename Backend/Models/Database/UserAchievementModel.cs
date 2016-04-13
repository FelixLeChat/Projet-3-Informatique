namespace Models.Database
{
    public class UserAchievementModel
    {
        public int Id { get; set; }
        public AchievementModel Achievement { get; set; }
        public bool IsDone { get; set; } = false;
    }
}