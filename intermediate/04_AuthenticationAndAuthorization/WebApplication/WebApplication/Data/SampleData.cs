using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication.Data
{
    public class SampleData
    {
        public static async Task InitializeData(IServiceProvider services, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("SampleData");

            using(var serviceScope = services.GetRequiredService<IServiceProvider>().CreateScope())
            {
                var env = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();
                if (!env.IsDevelopment()) return;

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                // Create our roles
                var adminTask = roleManager.CreateAsync(
                    new IdentityRole { Name = "Admin" });
                var powerUserTask = roleManager.CreateAsync(
                    new IdentityRole { Name = "Power Users" });
                Task.WaitAll(adminTask, powerUserTask);
                logger.LogInformation("==> Added Admin and Power Users roles");

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                // Create our default user
                var user = new ApplicationUser
                {
                    Email = "jeff@test.com",
                    UserName = "jeff@test.com"
                };
                await userManager.CreateAsync(user, "P@ssw0rd");
                logger.LogInformation($"==> Create user jeff@test.com with password Passw0rd");

                await userManager.AddToRoleAsync(user, "Admin");
                //await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Country, "Canada"));
            }
        }
    }
}
