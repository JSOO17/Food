using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Domain.Models;

namespace Foods.Application.Services.Interfaces
{
    public interface IDishServices
    {
        Task<List<DishResponseDTO>> GetDishes(int page, int count, long restaurantId);
        Task<DishResponseDTO> CreateDish(DishRequestDTO dish);
        Task UpdateDish(long id, DishRequestDTO dish, long userId);
    }
}
