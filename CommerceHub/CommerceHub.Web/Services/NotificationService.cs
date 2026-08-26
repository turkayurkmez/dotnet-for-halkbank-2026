namespace CommerceHub.Web.Services
{
    /*
     * Bir nesne GELİŞİME AÇIK, DEĞİŞİME KAPALIDIR
     */
    public enum NotificationType
    {
        Email,
        SMS,
        WhatsApp
    }
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }
        //Bu metot, OCP ilkesini ihlal eder. Gelecekte her yeni durum için case yazmak zorunda kalırız.
        //public void Notify(NotificationType type, string message) 
        //{
        //    switch (type)
        //    {
        //        case NotificationType.Email:
        //            _logger.LogInformation($"{message}, eposta olarak gönderildi");
        //            break;
        //        case NotificationType.SMS:
        //            _logger.LogInformation($"{message}, SMS olarak gönderildi");
        //            break;
        //        case NotificationType.WhatsApp:
        //            break;
        //        default:
        //            throw new NotSupportedException("Desteklenmeyen bildirim...");

        //    }

        //}

        //Yeni metotta, INotification hangi yöntemle (SMS, Email vs) gönderim yapılacağını BİLMİYOR. Ama eylemi (Send) biliyor:
        public void Notify(INotification notification, string message)
        {
            notification.Send(message);
        }

    }
}
