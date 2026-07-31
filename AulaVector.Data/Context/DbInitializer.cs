using Microsoft.AspNetCore.Identity;
using AulaVector.Models;

namespace AulaVector.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles =
        {
            "Admin",
            "Customer"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role
                });
            }
        }
    }
}