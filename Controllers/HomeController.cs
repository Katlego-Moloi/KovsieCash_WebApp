using KovsieCash_WebApp.Data;
using KovsieCash_WebApp.Models;
using KovsieCash_WebApp.Models.KovsieCash_WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace KovsieCash_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        [TempData] /*Assigned value will only be available until next request */
        public string Message { get; set; }
        public int iPageSize = 5;

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Dashboard");
            }
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            // Get the user ID from the claims
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve accounts linked to the user
            IEnumerable<Account> accounts = _repo.Accounts.GetAccountsByUserId(userId);

            // Pass the accounts to the view
            return View(accounts);
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
    }
}
