using AutoMapper;
using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Domain.Models;

namespace Foods.Application.Mappers
{
    public class RestaurantEmployeesMapper
    {
        public static RestaurantEmployeesResponseDTO ToResponse(RestaurantEmployeesModel restaurant)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RestaurantEmployeesModel, RestaurantEmployeesResponseDTO>());

            var mapper = new Mapper(config);

            return mapper.Map<RestaurantEmployeesModel, RestaurantEmployeesResponseDTO>(restaurant);
        }

        public static RestaurantEmployeesModel ToModel(RestaurantEmployeesRequestDTO restaurant)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RestaurantEmployeesRequestDTO, RestaurantEmployeesModel>());

            var mapper = new Mapper(config);

            return mapper.Map<RestaurantEmployeesRequestDTO, RestaurantEmployeesModel>(restaurant);
        }
    }
}
