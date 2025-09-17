using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Enums;

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToolCategory>().HasData(
                new ToolCategory { Id = 1, Name = "Elverktyg", Description = "Verktyg som drivs med el." },
                new ToolCategory { Id = 2, Name = "Handverktyg", Description = "Verktyg som används manuellt." },
                new ToolCategory { Id = 3, Name = "Trädgårdsverktyg", Description = "Verktyg för trädgårdsskötsel." },
                new ToolCategory { Id = 4, Name = "Måleriverktyg", Description = "Verktyg för målning och renovering." },
                new ToolCategory { Id = 5, Name = "Bilverktyg", Description = "Verktyg för fordon." }
            );

            modelBuilder.Entity<Tool>().HasData(
                new Tool { Id = 1, Name = "Borrhammare", Description = "Kraftfull borrhammare för betong och tegel.", RentalPrice = 250, Status = ToolStatus.Available, IsDeleted = false, ToolCategoryId = 1 },
                new Tool { Id = 2, Name = "Skruvdragare", Description = "Lättanvänd skruvdragare med två batterier.", RentalPrice = 120, Status = ToolStatus.Rented, IsDeleted = false, ToolCategoryId = 1 },
                new Tool { Id = 3, Name = "Såg", Description = "En robust såg för alla träprojekt.", RentalPrice = 50, Status = ToolStatus.Available, IsDeleted = false, ToolCategoryId = 2 },
                new Tool { Id = 4, Name = "Skiftnyckel-set", Description = "Komplett set med skiftnycklar i olika storlekar.", RentalPrice = 80, Status = ToolStatus.Available, IsDeleted = false, ToolCategoryId = 2 },
                new Tool { Id = 5, Name = "Gräsklippare", Description = "Effektiv gräsklippare för medelstora trädgårdar.", RentalPrice = 200, Status = ToolStatus.Available, IsDeleted = false, ToolCategoryId = 3 },
                new Tool { Id = 6, Name = "Lövblås", Description = "Kraftig lövblås för att snabbt rensa upp i trädgården.", RentalPrice = 150, Status = ToolStatus.UnderMaintenance, IsDeleted = false, ToolCategoryId = 3 }
            );

            // Användarnamn för admin
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin", Email = "admin@tooli.se", PasswordHash = "admin", Role = "Admin" }
            );
        }

    }
}
