using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KovsieCash_WebApp.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        public NotificationType Type { get; set; }

        public String NotificationDescription { get; set; }

        public DateTime NotificationDateTime { get; set; }

        public NotificationStatus Status { get; set; }

        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser UserToNotify { get; set; }


    }
}
