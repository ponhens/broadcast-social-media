using BroadcastSocialMedia.Models;
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
                Name = user.Name ?? "",
                ImageFilenameGUID = user.ProfileImageFilenameGUID,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileUpdateViewModel viewModel)
        {
            var fileNameGUID = SaveImageFileInServerFolderAndCreateGUIDForIt(viewModel.ImageFile);

            var user = await _userManager.GetUserAsync(User);

            user.Name = viewModel.Name;
            user.ProfileImageFilenameGUID = fileNameGUID;

           await _userManager.UpdateAsync(user);

            return Redirect("/");
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
