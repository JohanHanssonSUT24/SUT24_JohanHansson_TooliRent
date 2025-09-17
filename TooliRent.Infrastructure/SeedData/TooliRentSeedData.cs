using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Entities;
using TooliRent.Domain.Enums;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.SeedData
{
    public static class TooliRentSeedData
    {
        public static void Seed(TooliRentDbContext context)
        {
            if (context.ToolCategories.Any() || context.Tools.Any())
            {
                return;
            }

            var categories = new List<ToolCategory>
            {
                new ToolCategory { Name = "Elverktyg", Description = "Verktyg som drivs med el." },
                new ToolCategory { Name = "Handverktyg", Description = "Verktyg som används manuellt." },
                new ToolCategory { Name = "Trädgårdsverktyg", Description = "Verktyg för trädgårdsskötsel." },
                new ToolCategory { Name = "Måleriverktyg", Description = "Verktyg för målning och renovering." },
                new ToolCategory { Name = "Bilverktyg", Description = "Verktyg för fordon." }
            };

            context.ToolCategories.AddRange(categories);
            context.SaveChanges();

            var tools = new List<Tool>
            {
                new Tool { Name = "Borrhammare", Description = "Kraftfull borrhammare för betong och tegel.", RentalPrice = 250, Status = ToolStatus.Available, IsDeleted = false, ToolCategory = categories.First(c => c.Name == "Elverktyg") },
                new Tool { Name = "Skruvdragare", Description = "Lättanvänd skruvdragare med två batterier.", RentalPrice = 120, Status = ToolStatus.Rented, IsDeleted = false, ToolCategory = categories.First(c => c.Name == "Elverktyg") },
                new Tool { Name = "Såg", Description = "En robust såg för alla träprojekt.", RentalPrice = 50, Status = ToolStatus.Available, IsDeleted = false, ToolCategory = categories.First(c => c.Name == "Handverktyg") },
                new Tool { Name = "Skiftnyckel-set", Description = "Komplett set med skiftnycklar i olika storlekar.", RentalPrice = 80, Status = ToolStatus.Available, IsDeleted = false, ToolCategory = categories.First(c => c.Name == "Handverktyg") },
                new Tool { Name = "Gräsklippare", Description = "Effektiv gräsklippare för medelstora trädgårdar.", RentalPrice = 200, Status = ToolStatus.Available, IsDeleted = false, ToolCategory = categories.First(c => c.Name == "Trädgårdsverktyg") },
                new Tool { Name = "Lövblås", Description = "Kraftig lövblås för att snabbt rensa upp i trädgården.", RentalPrice = 150, Status = ToolStatus.UnderMaintenance, IsDeleted = false, ToolCategory = categories.First(c => c.Name == "Trädgårdsverktyg") }
            };

            context.Tools.AddRange(tools);
            context.SaveChanges();

            var user = new User
            {
                Name = "Admin",
                Email = "admin@tooli.se",
                PasswordHash = "admin",
                Role = "Admin"
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}