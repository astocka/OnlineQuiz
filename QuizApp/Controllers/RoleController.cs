using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizApp.Controllers
{
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

        //
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] IdentityRole<int> role)
        {
            if (ModelState.IsValid)
            {
                await RoleManager.CreateAsync(role);
                return RedirectToAction("Index", "Role");
            }

            return View(role);
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
