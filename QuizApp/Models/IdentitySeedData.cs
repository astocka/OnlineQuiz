using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QuizApp.Models
{
    public class IdentitySeedData
    {
        private const string AdminUser = "admin";
        private const string AdminPassword = "Admin-123";

        public static async Task Seed(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            AppUser user = await userManager.FindByNameAsync(AdminUser);
            if (user == null)
            {
                user = new AppUser("admin");
                IdentityResult result = await userManager.CreateAsync(user, AdminPassword);
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
