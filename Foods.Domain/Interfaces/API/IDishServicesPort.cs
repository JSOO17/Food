using Foods.Domain.Models;

namespace Foods.Domain.Interfaces.API
{
    public interface IDishServicesPort
    {
        Task<List<DishModel>> GetDishes(int page, int count, long restaurantId);
        Task<DishModel> CreateDish(DishModel dish);
        Task UpdateDish(long id, DishModel dish, long userId);
    }
}
