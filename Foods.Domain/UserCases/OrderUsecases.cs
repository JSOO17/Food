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

        public OrderUsecases(IOrderPersistencePort orderPersistence)
        {
            _orderPersistence = orderPersistence;
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

        private async Task ValidateOrder(OrderModel orderModel)
        {
            var validator = new ValidatorOrderModel();

            await validator.ValidateAndThrowAsync(orderModel);
        }
    }
}
