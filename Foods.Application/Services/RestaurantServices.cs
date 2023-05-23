using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Application.Mappers;
using Foods.Application.Services.Interfaces;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Models;

namespace Foods.Application.Services
{
    public class RestaurantServices : IRestaurantServices
    {
        private readonly IRestaurantServicesPort _restaurantServicesPort;

        public RestaurantServices(IRestaurantServicesPort restaurantServicesPort)
        {
            _restaurantServicesPort = restaurantServicesPort;
        }

        public async Task<RestaurantEmployeesResponseDTO> CreateEmployeeRestaurant(RestaurantEmployeesRequestDTO request, long userId)
        {
            var model = RestaurantEmployeesMapper.ToModel(request);

            var restaurantEmployee = await _restaurantServicesPort.CreateEmployeeRestaurant(model, userId);

            return RestaurantEmployeesMapper.ToResponse(restaurantEmployee);
        }

        public async Task<RestaurantResponseDTO> CreateRestaurant(RestaurantRequestDTO restaurantRequest)
        {
            var restaurantModel = RestaurantMapper.ToRestaurantModel(restaurantRequest);

            var restaurant =  await _restaurantServicesPort.CreateRestaurant(restaurantModel);

            return RestaurantMapper.ToRestaurantResponse(restaurant);
        }

        public async Task<List<ItemRestaurantResponseDTO>> GetRestaurants(int page, int count)
        {
            var restaurants = await _restaurantServicesPort.GetRestaurants(page, count);

            return RestaurantMapper.ToRestaurantResponse(restaurants);
        }
    }
}
