using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.SPI
{
    public interface IDishPersistencePort
    {
        Task<List<CategoryDishesModel>> GetDishes(int page, int count, long restaurantId);
        Task<DishModel> CreateDish(DishModel dish);
        Task UpdateDish(long id, DishModel dish);
        Task<RestaurantModel?> GetRestaurantByDishId(long dishId);
    }
}
