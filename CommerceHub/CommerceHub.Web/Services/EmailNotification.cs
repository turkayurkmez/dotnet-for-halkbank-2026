namespace CommerceHub.Web.Services
{
    public class EmailNotification : INotification
    {
           

        public void Send(string message)
        {
            Console.WriteLine($"Eposta gönderildi:{message}");
        }
    }
}
