using TimeFlow.DAL.Contexts;
using TimeFlow.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace TimeFlow.API.Infrastructure
{
    public static class PrepDb
    {
        public async static Task PrepDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                await SeedData(serviceScope);
            }
        }

        private async static Task SeedData(IServiceScope serviceScope)
        {
            Console.WriteLine("--> Preparing database...");

            serviceScope.ServiceProvider.GetRequiredService<IdentityContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
            var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new List<Category>
                {
                    new Category { Name = "Food" },
                    new Category { Name = "Transport" },
                    new Category { Name = "Entertainment" },
                    new Category { Name = "Health" },
                    new Category { Name = "Education" },
                    new Category { Name = "Other" }
                });

                context.SaveChanges();
            }
        }
    }
}
