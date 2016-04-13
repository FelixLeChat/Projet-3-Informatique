﻿using System;

namespace Models.Database
{
    public class UserDailyModel
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public string UserHashId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public EnumsModel.DailyType DailyType { get; set; }
    }
}