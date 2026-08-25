using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public class OrderService
    {
        public void PrintTotal(List<Order> orders)
        {
            foreach (var item in orders)
            {
                Console.WriteLine($"Toplam: {item.GetTotal()} ");
            }
        }
    }
}
