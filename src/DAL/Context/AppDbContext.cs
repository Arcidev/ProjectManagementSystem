using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    /// <summary>
    /// Database context for the app
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectTask> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>().HasIndex(x => x.Code).IsUnique();
        }
    }
}
