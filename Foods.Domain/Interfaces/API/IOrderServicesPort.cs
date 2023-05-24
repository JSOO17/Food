using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.API
{
    public interface IOrderServicesPort
    {
        Task<OrderModel> CreateOrder(OrderModel orderModel);
    }
}
