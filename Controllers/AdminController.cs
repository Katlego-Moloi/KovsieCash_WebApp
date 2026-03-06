using KovsieCash_WebApp.Data;
using KovsieCash_WebApp.Models;
using KovsieCash_WebApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KovsieCash_WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IRepositoryWrapper repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<ApplicationUserModel> applicationUsers = new List<ApplicationUserModel>();

            List<ApplicationUser> _applicationUsers = _repo.ApplicationUsers.GetUsersWithAccounts().ToList();

            foreach (ApplicationUser _applicationUser in _applicationUsers)
            {
                List<String> roles = (await _userManager.GetRolesAsync(_applicationUser)).ToList();

                applicationUsers.Add(new ApplicationUserModel
                {
                    Id = _applicationUser.Id,
                    UserName = _applicationUser.UserName,
                    Email = _applicationUser.Email,
                    PhoneNumber = _applicationUser.PhoneNumber,
                    Accounts = _applicationUser.Accounts,
                    UserType = roles.FirstOrDefault()
                });
            }

            return View(applicationUsers);
        }
    }
}
