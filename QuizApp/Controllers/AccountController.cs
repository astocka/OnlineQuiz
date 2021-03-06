﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Context;
using QuizApp.Models;
using QuizApp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizApp.Controllers
{
    public class AccountController : Controller
    {
        protected UserManager<AppUser> UserManager { get; }
        protected SignInManager<AppUser> SignInManager { get; }
        protected RoleManager<IdentityRole<int>> RoleManager;
        protected AppDbContext Context { get; }

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<int>> roleManager, AppDbContext context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            Context = context;

            IdentitySeedData.Seed(userManager, roleManager).Wait();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(signInViewModel.Login, signInViewModel.Password,
                    signInViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    if(signInViewModel.Login == "admin")
                        return RedirectToAction("Index", signInViewModel.Login);
                    return RedirectToAction("Index", "User");
                }


                ModelState.AddModelError("", "Wrong login or password");
            }

            return View(signInViewModel);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser(signUpViewModel.Login) { Email = signUpViewModel.Email, Name = signUpViewModel.Name };
                var result = await UserManager.CreateAsync(user, signUpViewModel.Password);
                if (result.Succeeded)
                {
                    await SignInManager.PasswordSignInAsync(signUpViewModel.Login, signUpViewModel.Password, true,
                        false);
                    return RedirectToAction("SignIn", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(signUpViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Account");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);
                var result = await UserManager.ChangePasswordAsync(user,
                    viewModel.CurrentPassword, viewModel.NewPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignOutAsync();
                    return RedirectToAction("SignIn", "Account");
                }
                ModelState.AddModelError("", "Wrong login or password");
            }
            return View(viewModel);
        }
    }
}
