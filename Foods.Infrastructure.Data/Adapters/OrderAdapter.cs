using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Domain.Utils;
using Foods.Infrastructure.Data.Mappers;
using Foods.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Foods.Infrastructure.Data.Adapters
{
    public class OrderAdapter : IOrderPersistencePort
    {
        private readonly foodContext _foodContext;

        public OrderAdapter(foodContext foodContext)
        {
            _foodContext = foodContext;
        }

        public async Task<OrderModel> CreateOrder(OrderModel orderModel)
        {
            var order = OrderMapper.ToOrder(orderModel);

            order.State = OrderStates.Pending;

            await _foodContext.Orders.AddAsync(order);



            var orderDishes = OrderMapper.ToOrderDishes(orderModel.Dishes);

            orderDishes.ForEach(d => d.OrderId = order.Id);

            await _foodContext.Orderdishes.AddRangeAsync(orderDishes);

            await _foodContext.SaveChangesAsync();

            orderModel.Id = order.Id;
            orderModel.Dishes = OrderMapper.ToModel(orderDishes);

            return orderModel;
        }

        public async Task<bool> HasClientOrders(long clientId)
        {
            var states = new List<string>()
            {
                OrderStates.Pending,
                OrderStates.InPreparation,
                OrderStates.Ready
            };

            return await _foodContext.Orders
                        .Where(o => o.ClientId == clientId && states.Contains(o.State))
                        .AnyAsync();
        }
    }
}
