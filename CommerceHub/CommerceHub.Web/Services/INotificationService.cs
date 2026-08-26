namespace CommerceHub.Web.Services
{
    public interface INotificationService
    {
        void Notify(INotification notification, string message);
    }
}