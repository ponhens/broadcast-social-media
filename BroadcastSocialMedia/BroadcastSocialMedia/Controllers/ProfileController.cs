﻿using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BroadcastSocialMedia.Controllers
{
    public class ProfileController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;

        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var viewModel = new ProfileIndexViewModel()
            {
                //"om user.Name är null, använd då en tom sträng ("")"
                Name = user.Name ?? "",
                ImageFilenameGUID = user.ProfileImageFilenameGUID,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileUpdateViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var anotherUserAlreadyHasThatName = _dbContext.Users.Any(u => u.Name == viewModel.Name);

            if (anotherUserAlreadyHasThatName == true) 
            {
                ModelState.AddModelError("Name", "Name is already taken.");

                var profileImageViewModel = new ProfileIndexViewModel()
                {
                    //"om user.Name är null, använd då en tom sträng ("")"
                    Name = viewModel.Name ?? "",
                    ImageFilenameGUID = user.ProfileImageFilenameGUID,
                    //ImageFilenameGUID = user.ProfileImageFilenameGUID,
                };

                return View("Index", profileImageViewModel);
            }

            if (viewModel.ImageFile != null)
            {
                var fileNameGUID = SaveImageFileInServerFolderAndCreateGUIDForIt(viewModel.ImageFile);
                user.ProfileImageFilenameGUID = fileNameGUID;
            }

            user.Name = viewModel.Name;

            

            await _userManager.UpdateAsync(user);

            return Redirect("/Profile");
        }

        private string SaveImageFileInServerFolderAndCreateGUIDForIt(IFormFile imageFile)
        {
            string projectDirectory = Directory.GetCurrentDirectory();
            string relativePath = "wwwroot\\images\\profilePictures";
            string fullPath = Path.Combine(projectDirectory, relativePath);
            var fileNameGUID = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string fullPathPlusImageFileName = Path.Combine(fullPath, fileNameGUID);

            using (var stream = new FileStream(fullPathPlusImageFileName, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return fileNameGUID;
        }
    }
}
