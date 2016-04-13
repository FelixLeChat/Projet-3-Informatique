using Models;

namespace FrontEnd.ViewModel
{
    public class GameViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public EnumsModel.GameState State { get; set; }
        public bool IsPrivate { get; set; }
        public string Population { get; set; }
        public int  Level { get; set; }
        public double Ping { get; set; }
        
    }
}