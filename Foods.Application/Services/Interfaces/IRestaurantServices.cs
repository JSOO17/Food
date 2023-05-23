using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Domain.Models;

namespace Foods.Application.Services.Interfaces
{
    public interface IRestaurantServices
    {
        Task<List<ItemRestaurantResponseDTO>> GetRestaurants(int page, int count);
        Task<RestaurantResponseDTO> CreateRestaurant(RestaurantRequestDTO restaurant);
        Task<RestaurantEmployeesResponseDTO> CreateEmployeeRestaurant(RestaurantEmployeesRequestDTO restaurant, long userId);
    }
}
