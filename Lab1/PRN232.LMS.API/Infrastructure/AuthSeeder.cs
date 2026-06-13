using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Services.Security;

namespace PRN232.LMS.API.Infrastructure;

public static class AuthSeeder
{
    public static async Task SeedAsync(LmsDbContext db, IServiceProvider services)
    {
        if (await db.Users.AnyAsync())
        {
            return;
        }

        var config = services.GetRequiredService<IConfiguration>();
        var hasher = services.GetRequiredService<IPasswordHashService>();

        var adminUsername = string.IsNullOrWhiteSpace(config["Admin:Username"]) ? "admin" : config["Admin:Username"]!;
        var adminPassword = string.IsNullOrWhiteSpace(config["Admin:Password"]) ? "123456" : config["Admin:Password"]!;

        db.Users.Add(new User
        {
            Username = adminUsername,
            PasswordHash = hasher.Hash(adminPassword),
            Role = "Admin",
        });

        await db.SaveChangesAsync();
    }
}
