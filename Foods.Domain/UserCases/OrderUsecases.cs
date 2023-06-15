using FluentValidation;
using Foods.Domain.Exceptions;
using Foods.Domain.HttpClients.Interfaces;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Domain.Utils;
using Foods.Domain.Utils.Validators;

namespace Foods.Domain.UserCases
{
    public class OrderUsecases : IOrderServicesPort
    {
        private readonly IOrderPersistencePort _orderPersistence;
        private readonly IRestaurantPersistencePort _restaurantPersistence;
        private readonly IMessengerHttpClient _messengerHttpClient;

        public OrderUsecases(IOrderPersistencePort orderPersistence, IRestaurantPersistencePort restaurantPersistence, IMessengerHttpClient messengerHttpClient)
        {
            _orderPersistence = orderPersistence;
            _restaurantPersistence = restaurantPersistence;
            _messengerHttpClient = messengerHttpClient;
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

        public async Task<List<OrderModel>> GetOrders(string state, int page, int count, long userId)
        {
            var restaurant = await _restaurantPersistence.GetRestaurantByEmployeeId(userId) ?? throw new UserIsNotAEmployeeException("You are not a employee");

            return await _orderPersistence.GetOrders(state, page, count, restaurant.Id);
        }

        public async Task<OrderModel> UpdateOrder(long id, OrderModel orderModel, long userId, string cellphoneUser)
        {
            var restaurant = await _restaurantPersistence.GetRestaurantByEmployeeId(userId) ?? throw new UserIsNotAEmployeeException("You are not a employee");

            if (restaurant.Id != orderModel.RestaurantId)
            {
                throw new RoleHasNotPermissionException("You are not a employee of the restaurant");
            }

            var state = await _orderPersistence.GetOrderState(id);

            ValidateOrderStates(state, orderModel.State);

            if (orderModel.State == OrderStates.Delivered)
            {
                await ValidateCode(id, orderModel.Code);
            }

            var order = await _orderPersistence.UpdateOrder(id, orderModel) ?? throw new ResourceDoesNotExistException();
            
            if (orderModel.State == OrderStates.Ready)
            {
                await ManageCode(order, cellphoneUser);
            }

            return order;
        }

        private async Task ManageCode(OrderModel orderModel, string cellphone)
        {
            var code = await GenerateCode();

            await _orderPersistence.UpdateOrderCode(orderModel.Id, code);

            await SendMessage(
                $"The order is ready, you can come here. Your code is {code}."
                , cellphone
           );
        }

        private async Task<string> GenerateCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            var codeExists = true;

            var code = string.Empty;

            while (codeExists) 
            {
                code = new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());

                codeExists = await _orderPersistence.CodeExists(code);
            }

            return code;
        }

        private async Task ValidateCode(long id, string code)
        {
            if (!await _orderPersistence.ValidateCode(id, code))
            {
                throw new OrderCodeInvalidException ();    
            }
        }

        private async Task SendMessage(string body, string cellphoneUser)
        {
            await _messengerHttpClient.SendMessageAsync(new MessageModel
            {
                Body = body,
                To = cellphoneUser
            });
        }

        private void ValidateOrderStates(string state, string newState)
        {
            if (state != OrderStates.Ready
                && newState == OrderStates.Delivered)
            {
                throw new OrderStateChangeInvalidException("The order is not ready yet");
            }

            if (state != OrderStates.Pending
                && newState == OrderStates.Canceled)
            {
                throw new OrderStateChangeInvalidException("The order is in preparation or ready rigth now");
            }
        }

        private async Task ValidateOrder(OrderModel orderModel)
        {
            var validator = new ValidatorOrderModel();

            await validator.ValidateAndThrowAsync(orderModel);
        }
    }
}
