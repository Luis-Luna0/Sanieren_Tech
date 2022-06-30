using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DOC_RASCH.Data.Entities;


namespace DOC_RASCH.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Business> Business { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Business>().HasIndex(x => x.Name).IsUnique();
        }
    }
}