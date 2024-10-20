namespace KovsieCash_WebApp.Models
{
    public enum NotificationType
    {
        Deposit,
        Withdrawal,
        Transfer,
        RoleRequest,
        Advice
    }

    public enum NotificationStatus
    {
        Unread,
        Read,
        Deleted
    }
}
