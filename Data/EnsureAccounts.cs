using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Zählerstände.Data;

public class EnsureAccounts {
    public static void Migrate(IApplicationBuilder app) {
        var ctx = app.ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ZählerständeDbContext>();
        if (ctx.Database.GetPendingMigrations().Any())
        {
            ctx.Database.Migrate();
        }
    }

    public static async Task SeedAccounts(IApplicationBuilder app) {
        //create roles
        var roleMgr = app.ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = ["Admin", "User"];
        foreach (var role in roles)
        { 
            if (!await roleMgr.RoleExistsAsync(role))
            { await roleMgr.CreateAsync(new IdentityRole(role)); }
        }

        var userMgr = app.ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();
        var admin = await userMgr.FindByNameAsync("Admin");
        if (admin is null)
        { admin = new IdentityUser() { UserName = "Admin" };

          await userMgr.CreateAsync(admin, "Secret123$");
          await userMgr.AddToRoleAsync(admin, "Admin"); }

        var user = await userMgr.FindByNameAsync("User");
        if (user is null)
        { user = new IdentityUser() { UserName = "User" };
        await userMgr.CreateAsync(user, "Secret123$");
        await userMgr.AddToRoleAsync(user, "User");
        }
    }
    
}