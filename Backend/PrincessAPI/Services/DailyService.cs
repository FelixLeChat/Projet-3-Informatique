using System;
using System.Linq;
using Models.Database;
using Models.Token;
using PrincessAPI.Infrastructure;

namespace PrincessAPI.Services
{
    public class DailyService : AbstractService
    {
        public DailyService(UserToken userToken) : base(userToken)
        {
        }

        public DailyModel GetDaily()
        {
            return GetTodayDaily();
        }

        public void CompleteDaily()
        {
            var daily = GetTodayDaily();

            using (var db = new SystemDBContext())
            {
                daily = db.Daylies.Find(daily.Id);
                daily.IsDone = true;
                db.SaveChanges();
            }
        }

        public void DeleteAllDaily()
        {
            using (var db = new SystemDBContext())
            {
                var dailies = db.Daylies.Where(x => x.UserHashId == UserToken.UserId).ToList();

                foreach (var daily in dailies)
                {
                    db.Daylies.Remove(daily);
                }
                db.SaveChanges();
            }
        }

        private DailyModel GetTodayDaily()
        {
            using (var db = new SystemDBContext())
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1).AddHours(-1);

                var daily =
                    db.Daylies.FirstOrDefault(
                        x => x.UserHashId == UserToken.UserId && x.BeginTime >= today && x.EndTime <= tomorrow);

                if (daily == null)
                {
                    daily = new DailyModel()
                    {
                        BeginTime = today,
                        EndTime = tomorrow,
                        IsDone = false,
                        UserHashId = UserToken.UserId,
                        DailyType = DailyModelHelper.GetDailyType(today)
                    };

                    db.Daylies.Add(daily);
                    db.SaveChanges();
                }
                return daily;
            }
        }
    }
}