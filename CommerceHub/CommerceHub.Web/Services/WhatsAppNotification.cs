namespace CommerceHub.Web.Services
{
    public class WhatsAppNotification : INotification
    {
        public void Send(string message)
        {
            Console.WriteLine($"Whatsapp ile mesaj gönderildi!!!!");
        }
    }
}
