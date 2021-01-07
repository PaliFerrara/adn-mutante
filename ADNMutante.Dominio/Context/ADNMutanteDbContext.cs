using ADNMutante.Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ADNMutante.Dominio.Context
{
    public class ADNMutanteDbContext : DbContext
    {
        public DbSet<ADNMutanteDB> ADNs { get; set; }
        public ADNMutanteDbContext(DbContextOptions<ADNMutanteDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=ADNMutanteDatabase.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<ADNMutanteDB>().ToTable("ADNs", "ADNMutante");
            modelBuilder.Entity<ADNMutanteDB>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CadenaADN).IsUnique();
                entity.Property(e => e.IsMutant);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
