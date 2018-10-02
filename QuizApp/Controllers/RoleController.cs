using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        protected UserManager<AppUser> UserManager { get; }
        protected SignInManager<AppUser> SignInManager { get; }
        protected RoleManager<IdentityRole<int>> RoleManager { get; }

        public RoleController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<int>> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<IdentityRole<int>> roles = RoleManager.Roles.ToList();
            ViewBag.Users = UserManager.Users.ToList();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddToUserRole(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound();
            await UserManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Index", "Role");
        }
    }
}
