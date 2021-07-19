using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.DataModel
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging()
                          .EnableDetailedErrors()
                          .LogTo(Console.WriteLine, LogLevel.Information);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .Property(u => u.Name)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(u => u.Age)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(u => u.Email)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(u => u.Password)
                        .IsRequired();

            modelBuilder.Entity<User>()
                        .Ignore(u => u.ConfirmPassword)
                        .Property(u => u.ConfirmPassword)
                        .IsRequired();


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
