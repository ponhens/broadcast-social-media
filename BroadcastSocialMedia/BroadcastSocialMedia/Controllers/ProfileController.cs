﻿using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BroadcastSocialMedia.Controllers
{
    public class ProfileController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var viewModel = new ProfileIndexViewModel()
            {
                //"om user.Name är null, använd då en tom sträng ("")"
                Name = user.Name ?? ""
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update( ProfileIndexViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            user.Name = viewModel.Name;

           await _userManager.UpdateAsync(user);

            return Redirect("/");
        }
    }
}
