using CheckInService.Models;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace CheckInService.DBContexts
{
    public class CheckInReadContextDB: DbContext
    {
        public DbSet<CheckInReadModel> CheckInReadModel { get; set; }

        public CheckInReadContextDB(DbContextOptions<CheckInReadContextDB> dbContext): base(dbContext) { }


        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CheckInReadModel>().HasKey(e => e.CheckInSerialNr);
        }
    }
}
