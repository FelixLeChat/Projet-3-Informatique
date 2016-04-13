using System;
using System.Data.Entity;
using Models.Database;

namespace PrincessAPI.Infrastructure
{
    public class SystemDBContext : DbContext
    {
        public SystemDBContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>()
                        .Configure(c => c.HasColumnType("datetime2"));
        }

        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<ProfileModel> Profiles { get; set; }
        public virtual DbSet<MapModel> Maps { get; set; }
        public virtual DbSet<FriendModel> Friends { get; set; }
        public virtual DbSet<LeaderboardModel> Leaderboards { get; set; }
        public virtual DbSet<DailyModel> Daylies { get; set; }
    }
}