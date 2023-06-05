using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.API
{
    public interface IOrderServicesPort
    {
        Task<OrderModel> CreateOrder(OrderModel orderModel);
        Task<OrderModel> UpdateOrder(long id, OrderModel orderModel, long userId, string cellphoneUser);
        Task<List<OrderModel>> GetOrders(OrderFiltersModel filters, int page, int count, long userId);
    }
}
