using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
                var dbUser = await _dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

                //man måste tydligen ha denna koden för att kunna hämta Listening to. Koden precis ovan räcker inte trots att den innehåller samma delar. Konstigt!?
                var broadcasts = await _dbContext.Users.Where(u => u.Id == user.Id)
                    .SelectMany(u => u.ListeningTo)
                    .SelectMany(u => u.Broadcasts)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.Published)
                    .ToListAsync();

                var viewModel = new HomeIndexViewModel()
                {
                    Broadcasts = broadcasts
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
            var wwwrootPath = _hostingEnvironment.WebRootPath + "\\images\\broadcastImages";
            var fileNameGUID = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImageFile.FileName);
            var filePath = Path.Combine(wwwrootPath, fileNameGUID);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                viewModel.ImageFile.CopyTo(stream);
            }

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
    }
}
