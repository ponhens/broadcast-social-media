using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BroadcastSocialMedia.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(UsersIndexViewModel viewModel)
        {
            if(viewModel.Search != null)
            {
                var users = await _dbContext.Users.Where(u => u.Name.Contains(viewModel.Search))
                    .ToListAsync();
                viewModel.Result = users;
            }


            return View(viewModel);
        }

        [Route("/Users/{id}")]
        public async Task<IActionResult> ShowUser(string id)
        {
            var broadCasts = await _dbContext.Broadcasts.Where(b => b.User.Id == id).OrderByDescending(b => b.Published).ToListAsync();
            //Såhär skrev kursledaren// var user = await _userManager.GetUserAsync(User); // men jag tror att han gjorde fel så jag skrev som (se nedan) 
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);


            var viewModel = new UsersShowUserViewModel()
            {
                Broadcasts = broadCasts,
                User = user,
            };

            return View(viewModel);
        }
    }
}
