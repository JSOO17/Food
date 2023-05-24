using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.SPI
{
    public interface IOrderPersistencePort
    {
        Task<OrderModel> CreateOrder(OrderModel orderModel);
        Task<List<OrderModel>> GetOrders(OrderFiltersModel filters, int page, int count, long restaurantId);
        Task<bool> HasClientOrders(long clientId);
    }
}
