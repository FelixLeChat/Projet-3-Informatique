using System.Drawing;

namespace Models.Frontend
{
    public class ZoneModelListItem
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string MapImage { get; set; }
        public string HashId { get; set; }
        public bool NeedToUpdateImage { get; set; }
    }
}
