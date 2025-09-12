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
            if(!await context.ToolCategories.AnyAsync())
            {
                var powerTools = new ToolCategory { Name = "Powertools" };
                var regularTools = new ToolCategory { Name = "Regular tools" };
                await context.ToolCategories.AddRangeAsync(powerTools, regularTools);
                await context.SaveChangesAsync();

                if( !await context.Tools.AnyAsync())
                {
                    var tools = new List<Tool>
                    {
                        new Tool
                        {
                            Name = "Slaghammare Bosch",
                            Description = "Kraftfull borr för sten och cement",
                            DailyRentalPrice = 100.00m,
                            ToolCategoryId = powerTools.Id
                        },
                        new Tool
                        {
                            Name = "Sticksåg DeWalt",
                            Description = "Sticksåg med utbyttbart blad",
                            DailyRentalPrice = 99.00m,
                            ToolCategoryId = powerTools.Id
                        },
                        new Tool
                        {
                            Name = "Vinkelslip Makita",
                            Description = "Vinkelslip med 25cm klinga",
                            DailyRentalPrice = 110.00m,
                            ToolCategoryId = powerTools.Id
                        },
                        new Tool
                        {
                            Name = "Hammare",
                            Description = "Hammare, 11cm, Härdat stål",
                            DailyRentalPrice = 50.00m,
                            ToolCategoryId = regularTools.Id
                        },
                        new Tool
                        {
                            Name = "Såg",
                            Description = "Fogsvans",
                            DailyRentalPrice = 45.00m,
                            ToolCategoryId = regularTools.Id
                        },
                        new Tool
                        {
                            Name = "Tumstock",
                            Description = "2m",
                            DailyRentalPrice = 11.00m,
                            ToolCategoryId = regularTools.Id
                        }
                    };
                    await context.Tools.AddRangeAsync(tools);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
