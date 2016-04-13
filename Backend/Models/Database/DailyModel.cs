using System;

namespace Models.Database
{
    public class DailyModel
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public string UserHashId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public EnumsModel.DailyType DailyType { get; set; }
    }

    public class DailyModelHelper
    {
        public static EnumsModel.DailyType GetDailyType(DateTime beginTime)
        {
            return (EnumsModel.DailyType) beginTime.DayOfWeek;
        }
    }
}