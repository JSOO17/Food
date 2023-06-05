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

            await _foodContext.SaveChangesAsync();

            var orderDishes = OrderMapper.ToOrderDishes(orderModel.Dishes);

            orderDishes.ForEach(d => d.OrderId = order.Id);

            await _foodContext.Orderdishes.AddRangeAsync(orderDishes);

            await _foodContext.SaveChangesAsync();

            orderModel.Id = order.Id;
            orderModel.Dishes = OrderMapper.ToModel(orderDishes);

            return orderModel;
        }

        public async Task<List<OrderModel>> GetOrders(OrderFiltersModel filters, int page, int count, long restaurantId)
        {
            var skip = page > 1 ? count * (page - 1) : 0;

            var orders = await _foodContext.Orders
                                    .Where(d => d.RestaurantId == restaurantId && d.State == filters.State)
                                    .Skip(skip)
                                    .Take(count)
                                    .ToListAsync();
            var ordersModel = OrderMapper.ToModel(orders);

            ordersModel.ForEach(o =>
            {
                var dishes = _foodContext.Orderdishes.Where(d => d.OrderId == o.Id).ToList();
                o.Dishes = OrderMapper.ToModel(dishes);
            });

            return ordersModel;
        }

        public async Task<string> GetOrderState(long id)
        {
            return await _foodContext.Orders
                                        .Where(o => o.Id == id)
                                        .Select(o => o.State)
                                        .FirstOrDefaultAsync() ?? string.Empty;
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

        public async Task<OrderModel?> UpdateOrder(long id, OrderModel orderModel)
        {
            var order = await _foodContext.Orders.Where(dish => dish.Id == id).FirstOrDefaultAsync();

            if (order != null)
            {
                order.State = orderModel.State;
                order.ChefId = orderModel.ChefId;

                _foodContext.Entry(order).State = EntityState.Modified;

                await _foodContext.SaveChangesAsync();

                return OrderMapper.ToModel(order);
            }

            return null;
        }

        public async Task<bool> CodeExists(string code)
        {
            return await _foodContext.Orders
                               .Where(o => o.Code == code)
                               .AnyAsync();
        }

        public async Task<bool> ValidateCode(long id, string code)
        {
            var codeOrder = await _foodContext.Orders
                               .Where(o => o.Id == id)
                               .Select(o => o.Code)
                               .FirstOrDefaultAsync();

            return code == codeOrder;
        }

        public async Task UpdateOrderCode(long id, string code)
        {
            var order = await _foodContext.Orders.Where(dish => dish.Id == id).FirstOrDefaultAsync();

            if (order != null)
            {
                order.Code = code;

                _foodContext.Entry(order).State = EntityState.Modified;

                await _foodContext.SaveChangesAsync();
            }
        }
    }
}
