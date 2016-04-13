using System.Collections.Generic;

namespace Models.Database
{
    public class CommChannelModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HashId { get; set; }
        public List<string> Participants { get; set; } = new List<string>();
    }
}