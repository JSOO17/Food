using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.SPI
{
    public interface IOrderPersistencePort
    {
        Task<OrderModel> CreateOrder(OrderModel orderModel);
        Task<bool> HasClientOrders(long clientId);
    }
}
