using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Entities;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.SeedData
{
    public static class TooliRentSeedData
    {
        public static async Task EnsureSeedData(TooliRentDbContext context)
        {
            if(!await context.Tools.AnyAsync())
            {
                var tool1 = new Tool
                {
                    Name = "Slagborr",
                    Description = "Borrmaskin för att borra i betong och sten.",
                    DailyRentalPrice = 100.00m
                };
            }
        }
    }
}
