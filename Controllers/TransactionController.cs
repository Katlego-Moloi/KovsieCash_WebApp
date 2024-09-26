using KovsieCash_WebApp.Data;
using KovsieCash_WebApp.Models;
using KovsieCash_WebApp.Models.KovsieCash_WebApp.Models;
using KovsieCash_WebApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KovsieCash_WebApp.Controllers
{
	public class TransactionController : Controller
	{
		private readonly IRepositoryWrapper _repo;
		private readonly UserManager<ApplicationUser> _userManager;
		public TransactionController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		public IActionResult List(string id)
		{
			
			return View(_repo.Accounts.GetAccountWithHistory(id));
		}

		[HttpGet]
		public IActionResult Add(string id)
		{
			ViewBag.Id = id;

			// Get the user ID from the claims
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Retrieve accounts linked to the user
			IEnumerable<Account> accounts = _repo.Accounts.GetAccountsByUserId(userId);

			ViewBag.Id = id;
			ViewBag.Accounts = accounts;

			// Pass the accounts to the view
			return View("Add", new TransferModel());
		}

		[HttpPost]
		public IActionResult Add(TransferModel transfer)
		{
			_repo.Transactions.AddTransaction(transfer.fromAccNumber, transfer.toAccNumber, transfer.Amount, transfer.Reference);
			_repo.Save();

            return RedirectToAction("List", new { id = transfer.fromAccNumber });
        }
	}
}
