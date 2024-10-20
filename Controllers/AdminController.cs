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

        public IActionResult Index()
        {
            List<ApplicationUserModel> applicationUsers = new List<ApplicationUserModel>();

            IEnumerable<ApplicationUser> _applicationUsers = _repo.ApplicationUsers.GetUsersWithAccounts();

            foreach (ApplicationUser _applicationUser in _applicationUsers)
            {
                List<String> roles = _userManager.GetRolesAsync(_applicationUser).Result.ToList();

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
