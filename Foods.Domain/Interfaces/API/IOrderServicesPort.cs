using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.API
{
    public interface IOrderServicesPort
    {
        Task<OrderModel> CreateOrder(OrderModel orderModel);
        Task<List<OrderModel>> GetOrders(OrderFiltersModel filters, int page, int count, long userId);
    }
}
