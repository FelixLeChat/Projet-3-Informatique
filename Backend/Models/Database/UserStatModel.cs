namespace Models.Database
{
    public class UserStatModel
    {
        public int Id { get; set; }
        public StatModel Stat { get; set; }
        public int Progres { get; set; } = 0;
    }
}