using Microsoft.EntityFrameworkCore;

namespace DiveLog.Models
{
    public class DiveContext : DbContext
    {
        public DiveContext(DbContextOptions options) : base(options) { }
        public DbSet<Diver> Divers {get; set;}
        public DbSet<DiveSite> DiveSites {get; set;}
        public DbSet<DiverLog> DiverLogs {get; set;}
    }
}