using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KovsieCash_WebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using KovsieCash_WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using KovsieCash_WebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Serialization;

namespace KovsieCash_WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IRepositoryWrapper _repo;



		private readonly string _role = "Customer";

        public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment,
		IRepositoryWrapper repo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
            _repo = repo;
        }

		public IActionResult Details(int monthOfReport, string userId)
		{
			ApplicationUser user = _repo.ApplicationUsers.FindByCondition(u => u.Id == userId).FirstOrDefault();

			// Get transactions for the selected month
			IEnumerable<Transaction> transactionsThisMonth = _repo.Transactions.FindByCondition(t => t.Account.UserId == userId && t.DateTime.Month == monthOfReport && t.DateTime.Year == DateTime.Now.Year)
				.ToList();

			// Get transactions for the previous month
			int previousMonth = monthOfReport == 1 ? 12 : monthOfReport - 1;
			IEnumerable<Transaction> transactionsLastMonth = _repo.Transactions.FindByCondition(t => t.Account.UserId == userId && t.DateTime.Month == previousMonth && t.DateTime.Year == DateTime.Now.Year)
				.ToList();

            List<SelectListItem> monthsAvailable = new List<SelectListItem>();
            double[] transactionsByMonth = new double[13];
            double[] transactionsByDay = new double[32];


            for (int i = 1; i < 13; i++)
            {
                transactionsByMonth[1] = 0;

                if (_repo.Transactions.FindByCondition(t => t.DateTime.Month == i).Count() != 0)
                {
                    foreach (Transaction item in _repo.Transactions.FindByCondition(t => t.DateTime.Month == i))
                    {
                        transactionsByMonth[i] += item.Amount;

                        transactionsByDay[item.DateTime.Day] += item.Amount;
                    }

                    monthsAvailable.Add(new SelectListItem
                    {
                        Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
                        Value = i.ToString()
                    });
                }
                
            }

            ReportViewModel _report = new ReportViewModel
			{
				User = user,
				TransactionsThisMonth = transactionsThisMonth,
				TransactionsLastMonth = transactionsLastMonth,
				MonthsAvailable = monthsAvailable,
                SpendingByMonth = transactionsByMonth,
                SpendingByDay = transactionsByDay

			};

			return View(_report);
		}


		[HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            //if (ModelState.IsValid)
            //{

                ApplicationUser user =
                  await _userManager.FindByEmailAsync(loginModel.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,
                      loginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (User.IsInRole("Customer"))
                        {
                            return Redirect("/Home/Dashboard");
                        }
                        else
                        {
                            return Redirect("/Admin/Index");
                        }
                    }
                }
            //}
            //ModelState.AddModelError("", "Invalid email or password");
            return View(loginModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.FindByNameAsync(_role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(_role));
                }

                ApplicationUser user = new ApplicationUser
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password);
                ApplicationUser newUser = _repo.ApplicationUsers.FindByCondition(a => a.Email == registerModel.Email).First();

                if (result.Succeeded)
                {
                    // Handle Profile Picture Upload
                    if (registerModel.ProfilePicture != null && registerModel.ProfilePicture.Length > 0)
                    {
                        // Ensure the ProfilePictures directory exists
                        string imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profilepictures");
                        if (!Directory.Exists(imagesPath))
                        {
                            Directory.CreateDirectory(imagesPath);
                        }

                        // Use the user's id as the file name (sanitized)
                        string fileName = $"{newUser.Id}{Path.GetExtension(registerModel.ProfilePicture.FileName)}";
                        string filePath = Path.Combine(imagesPath, fileName);

                        // Update Profile Picture
                        newUser.ProfilePicture = fileName;
                        _repo.ApplicationUsers.Update(newUser);
                        _repo.Save();

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await registerModel.ProfilePicture.CopyToAsync(stream);
                        }


                        await _userManager.AddToRoleAsync(user, _role);

                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Unable to register new user");
                }
            }

            return View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
