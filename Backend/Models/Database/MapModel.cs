using System;

namespace Models.Database
{
    public class MapModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatorhashId { get; set; }
        public string Content { get; set; }
        public int Level { get; set; }
        public string HashId { get; set; }
        public string CreationDate { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class MapModelHelper
    {
        public const int MinLevel = 0;
        public const int MaxLevel = 4;

        public static bool IsValid(MapModel map)
        {
            if (string.IsNullOrWhiteSpace(map.Name))
                return false;
            if (string.IsNullOrWhiteSpace(map.Content))
                return false;
            if (map.Level < MinLevel || map.Level > MaxLevel)
                return false;
            return true;
        }
    }
}