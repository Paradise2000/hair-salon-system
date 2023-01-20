using Microsoft.EntityFrameworkCore;

namespace projekt_programowanie.Entities
{
    public class ProjektDbContext : DbContext
    {
        public ProjektDbContext(DbContextOptions<ProjektDbContext> options) : base(options) 
        {

        }

        public DbSet<BookedVisit> BookedVisits { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<WorkerAvailability> WorkersAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Role");
        }
    }
}
