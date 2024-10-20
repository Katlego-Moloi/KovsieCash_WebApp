using Microsoft.AspNetCore.Mvc;
using KovsieCash_WebApp.Data;
using KovsieCash_WebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace KovsieCash_WebApp.Controllers
{
    public class NotificationController : Controller
    {
        IRepositoryWrapper _repo;
        UserManager<ApplicationUser> _userManager;

        public NotificationController(IRepositoryWrapper repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public IActionResult List()
        {
            IEnumerable<Notification> notifications = _repo.Notifications.FindByCondition(n => n.UserID == _userManager.GetUserAsync(User).Result.Id);

            foreach (Notification _notification in notifications) 
            {
                if (_notification.Type != NotificationType.Advice && _notification.Type != NotificationType.RoleRequest)
                {
                    _notification.Status = NotificationStatus.Read;
                    _repo.Notifications.Update(_notification);

                }
            }

            _repo.Save();

            return View(notifications);
        }
    }
}
