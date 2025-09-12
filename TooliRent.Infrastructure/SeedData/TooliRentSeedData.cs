using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Entities;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.SeedData
{
    public static class TooliRentSeedData
    {
        public static void Seed(TooliRentDbContext context)
        {
            if (context.Tools.Any())
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

            context.ToolCategories.AddRange(categories); // Ändrad
            context.SaveChanges(); // Ändrad

            var tools = new List<Tool>();
            var random = new System.Random();

            for (int i = 1; i <= 30; i++)
            {
                var randomCategory = categories[random.Next(categories.Count)];
                tools.Add(new Tool
                {
                    Name = $"Verktyg {i}",
                    Description = $"Beskrivning för verktyg {i}. Kategorin är {randomCategory.Name}.",
                    RentalPrice = (decimal)(random.Next(50, 500)),
                    ToolCategory = randomCategory
                });
            }

            context.Tools.AddRange(tools); // Ändrad
            context.SaveChanges(); // Ändrad

            var user = new User
            {
                Name = "Admin",
                Email = "admin@tooli.se",
                PasswordHash = "admin"
            };

            context.Users.Add(user); // Ändrad
            context.SaveChanges(); // Ändrad
        }
    }
}