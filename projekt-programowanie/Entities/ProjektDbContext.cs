using Microsoft.EntityFrameworkCore;

namespace projekt_programowanie.Entities
{
    public class ProjektDbContext : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=ProjektDb;Trusted_Connection=True;";

        public DbSet<BookedVisit> BookedVisits { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerAvailability> WorkersAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }


}
