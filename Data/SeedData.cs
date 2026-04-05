using Animatch.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Animatch.Data
{
    public static class SeedData
    {
        private const string UserRole = "User";
        private const string AdminRole = "Administrator";

        public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await EnsureRoleAsync(roleManager, UserRole);
            await EnsureRoleAsync(roleManager, AdminRole);
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

    }
}
