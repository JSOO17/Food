using FluentValidation;
using Foods.Domain.Exceptions;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Domain.Utils.Validators;

namespace Foods.Domain.UserCases
{
    public class OrderUsecases : IOrderServicesPort
    {
        private readonly IOrderPersistencePort _orderPersistence;
        private readonly IRestaurantPersistencePort _restaurantPersistence;

        public OrderUsecases(IOrderPersistencePort orderPersistence, IRestaurantPersistencePort restaurantPersistence)
        {
            _orderPersistence = orderPersistence;
            _restaurantPersistence = restaurantPersistence;
        }

        public async Task<OrderModel> CreateOrder(OrderModel orderModel)
        {
            await ValidateOrder(orderModel);

            var clientHasOrders = await _orderPersistence.HasClientOrders(orderModel.ClientId);

            if (clientHasOrders)
            {
                throw new ClientAlreadyHasOrderException("You already have a order on pending, In Progress or Ready");
            }

            return await _orderPersistence.CreateOrder(orderModel);
        }

        public async Task<List<OrderModel>> GetOrders(OrderFiltersModel filters, int page, int count, long userId)
        {
            var restaurant = await _restaurantPersistence.GetRestaurantByEmployeeId(userId) ?? throw new UserIsNotAEmployeeException("You are not a employee");

            return await _orderPersistence.GetOrders(filters, page, count, restaurant.Id);
        }

        private async Task ValidateOrder(OrderModel orderModel)
        {
            var validator = new ValidatorOrderModel();

            await validator.ValidateAndThrowAsync(orderModel);
        }
    }
}
