using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public class OrderService : IOrderService
    {
        public void PrintTotal(List<IPricableOrder> orders)
        {
            foreach (var item in orders)
            {
                Console.WriteLine($"Toplam: {item.GetTotal()} ");
            }
        }
    }
}
