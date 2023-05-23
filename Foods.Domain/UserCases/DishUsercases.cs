using FluentValidation;
using Foods.Domain.Exceptions;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Interfaces.SPI;
using Foods.Domain.Models;
using Foods.Domain.Utils.Validators;

namespace Foods.Domain.UserCases
{
    public class DishUsercases : IDishServicesPort
    {
        private readonly IDishPersistencePort _dishPersistence;

        public DishUsercases(IDishPersistencePort dishPersistence)
        {
            _dishPersistence = dishPersistence;
        }

        public async Task<DishModel> CreateDish(DishModel dish)
        {
            ValidateDish(dish);

            return await _dishPersistence.CreateDish(dish);
        }

        public async Task<List<CategoryDishesModel>> GetDishes(int page, int count, long restaurantId)
        {
            return await _dishPersistence.GetDishes(page, count, restaurantId);
        }

        public async Task UpdateDish(long id, DishModel dish, long userId)
        {
            var restaurant = await _dishPersistence.GetRestaurantByDishId(id);

            if (restaurant != null && restaurant.OwnerId != userId)
            {
                throw new RoleHasNotPermissionException("You are not the owner of the restaurant");
            }

            await _dishPersistence.UpdateDish(id, dish);
        }

        private void ValidateDish(DishModel dish)
        {
            var validator = new ValidatorDishModel();

            validator.ValidateAndThrow(dish);
        }
    }
}
