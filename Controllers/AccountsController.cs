using Microsoft.AspNetCore.Mvc;

namespace KovsieCash_WebApp.Controllers
{
	public class AccountsController : Controller
	{
		public IActionResult Info()
		{
			return View();
		}

		public IActionResult Transfer()
		{
			return View();
		}
	}
}
