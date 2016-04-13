using System.IO.Packaging;

namespace FrontEnd.Game
{
    public class Zone
    {
        public string HashId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ImagePath { get; set; } 
        public int Level { get; set; }
        public long LastUpdateTimestamp { get; set; }
    }
}