namespace CommerceHub.Web.Services
{
    public interface INotification
    {
        void Send(string message);
    }

    public class EmailNotification : INotification
    {
           

        public void Send(string message)
        {
            Console.WriteLine($"Eposta gönderildi:{message}");
        }
    }

    public class SMSNotification : INotification
    {     

        public void Send(string message)
        {
            Console.WriteLine($"SMS gönderildi:{message}");

        }
    }

    public class WhatsAppNotification : INotification
    {
        public void Send(string message)
        {
            Console.WriteLine($"Whatsapp ile mesaj gönderildi!!!!");
        }
    }
}
