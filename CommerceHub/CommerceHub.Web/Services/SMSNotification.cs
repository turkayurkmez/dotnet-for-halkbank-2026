namespace CommerceHub.Web.Services
{
    public class SMSNotification : INotification
    {     

        public void Send(string message)
        {
            Console.WriteLine($"SMS gönderildi:{message}");

        }
    }
}
