using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace BroadcastSocialMedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) 
            {
                //var dbUser = await _dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

                //man måste tydligen ha denna koden för att kunna hämta Listening to. Koden precis ovan räcker inte trots att den innehåller samma delar. Konstigt!?
                var broadcasts = await _dbContext.Users.Where(u => u.Id == user.Id)
                    .SelectMany(u => u.ListeningTo)
                    .SelectMany(u => u.Broadcasts)
                    .Include(b => b.UserThatLike)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.Published)
                    .ToListAsync();

                string projectDirectory = Directory.GetCurrentDirectory();
                string relativePath = "wwwroot\\images\\broadcastImages\\";
                string fullPath = Path.Combine(projectDirectory, relativePath);

                var viewModel = new HomeIndexViewModel()
                {
                    Broadcasts = broadcasts,
                };

                return View(viewModel);
            }

            return View();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Broadcast(HomeBroadcastViewModel viewModel)
        {
            var fileNameGUID = SaveImageFileInServerFolderAndCreateGUIDForIt(viewModel.ImageFile);

            var user = await _userManager.GetUserAsync(User);
            var broadcast = new Broadcast()
            {
                Message = viewModel.Message,
                User = user,
                ImageFilenameGUID = fileNameGUID,
            };

            _dbContext.Broadcasts.Add(broadcast);
            await _dbContext.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> LikeBroadcast(HomeLikeBroadcastViewModel viewModel)
        {

            var user = await _userManager.GetUserAsync(User);
            var broadcast = await _dbContext.Broadcasts.FirstOrDefaultAsync(b => b.Id == viewModel.BroadcastId);
            var userAlreadyLike = await _dbContext.Broadcasts.Where(b => b.Id == viewModel.BroadcastId)
                .SelectMany(b => b.UserThatLike).Where(u => u.UserId == user.Id).FirstOrDefaultAsync();

            if (userAlreadyLike == null)
            {
                var userThatLikeBroadcast = new UserThatLikeBroadcast
                {
                    UserId = user.Id,
                    NameOfUserThatLike = user.Name,
                    BroadcastID = broadcast.Id,
                };

            broadcast.UserThatLike.Add(userThatLikeBroadcast);
                await _dbContext.SaveChangesAsync();
            }

            return Redirect("/");
        }

        private string SaveImageFileInServerFolderAndCreateGUIDForIt(IFormFile imageFile)
        {
            string projectDirectory = Directory.GetCurrentDirectory();
            string relativePath = "wwwroot\\images\\broadcastImages";
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
