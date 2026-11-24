using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MediaTracker
{
    public class AppDbContext : DbContext
    {
        public DbSet<Work> works { get; set; }
        public DbSet<Film> films { get; set; }
        public DbSet<Serial> serials { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;" +
                "Port=5432;" +
                "Database=MediaTracker;" +
                "Username=postgres;" +
                "Password=MediaTracker;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Work>().UseTptMappingStrategy();

            modelBuilder.Entity<Film>().ToTable("films");

            modelBuilder.Entity<Serial>().ToTable("serials");
        }
    }
}
