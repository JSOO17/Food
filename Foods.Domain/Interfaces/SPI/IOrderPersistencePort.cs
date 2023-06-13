using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.SPI
{
    public interface IOrderPersistencePort
    {
        Task<OrderModel> CreateOrder(OrderModel orderModel);
        Task<OrderModel?> UpdateOrder(long id, OrderModel orderModel);
        Task UpdateOrderCode(long id, string code);
        Task<List<OrderModel>> GetOrders(string state, int page, int count, long restaurantId);
        Task<string> GetOrderState(long id);
        Task<bool> HasClientOrders(long clientId);
        Task<bool> CodeExists(string code);
        Task<bool> ValidateCode(long id, string code);
    }
}
