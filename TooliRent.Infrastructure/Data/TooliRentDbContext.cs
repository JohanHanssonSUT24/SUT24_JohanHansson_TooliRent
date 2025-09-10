using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;

namespace TooliRent.Infrastructure.Data
{
    public class TooliRentDbContext : DbContext
    {
        public TooliRentDbContext(DbContextOptions<TooliRentDbContext> options) : base(options)
        {
        }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolCategory> ToolCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@tooli.se",
                    PasswordHash = "admin",
                    Role = "Admin"

                }
                );
            modelBuilder.Entity<ToolCategory>().HasData(
                new ToolCategory { Id = 1, Name = "Elverktyg", Description = "Verktyg som drivs med el." },
                new ToolCategory { Id = 2, Name = "Handverktyg", Description = "Verktyg som används manuellt." },
                new ToolCategory { Id = 3, Name = "Trädgårdsverktyg", Description = "Verktyg för trädgårdsskötsel." }
                );
        }

    }
}
