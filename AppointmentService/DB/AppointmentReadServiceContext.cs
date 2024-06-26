using AppointmentService.Domain;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace AppointmentService.DB
{
    public class AppointmentReadServiceContext : DbContext
    {
        public DbSet<AppointmentRead> appointmentsRead { get; set; }

        public AppointmentReadServiceContext(DbContextOptions<AppointmentReadServiceContext> options) : base(options) { }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }
    }
}
