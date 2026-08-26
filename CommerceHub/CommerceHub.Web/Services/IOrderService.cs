using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public interface IOrderService
    {
        void PrintTotal(List<IPricableOrder> orders);
    }
}