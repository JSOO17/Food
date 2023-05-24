using FluentValidation;
using Foods.Domain.Exceptions;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Domain.Utils.Validators;

namespace Foods.Domain.UserCases
{
    public class RestaurantUsercases : IRestaurantServicesPort
    {
        private readonly IRestaurantPersistencePort _restaurantPersistencePort;

        public RestaurantUsercases(IRestaurantPersistencePort restaurantPersistencePort)
        {
            _restaurantPersistencePort = restaurantPersistencePort;
        }

        public async Task<RestaurantEmployeesModel> CreateEmployeeRestaurant(RestaurantEmployeesModel model, long userId)
        {
            if (!await _restaurantPersistencePort.IsOwnerByRestaurant(model.RestaurantId, userId))
            {
                throw new RoleHasNotPermissionException("You are not the owner of the restaurant");
            }

            ValidateRestaurantEmployee(model);

            return await _restaurantPersistencePort.CreateEmployeeRestaurant(model);
        }

        public async Task<RestaurantModel> CreateRestaurant(RestaurantModel restaurant)
        {
            ValidateRestaurant(restaurant);

            return await _restaurantPersistencePort.CreateRestaurant(restaurant);
        }

        public async Task<List<ItemRestaurantModel>> GetRestaurants(int page, int count)
        {
            return await _restaurantPersistencePort.GetRestaurants(page, count);
        }

        private void ValidateRestaurant(RestaurantModel restaurant)
        {
            var validator = new ValidatorRestaurantModel();

            validator.ValidateAndThrow(restaurant);
        }

        private void ValidateRestaurantEmployee(RestaurantEmployeesModel restaurantEmployee)
        {
            var validator = new ValidatorRestaurantEmployeeModel();

            validator.ValidateAndThrow(restaurantEmployee);
        }
    }
}
