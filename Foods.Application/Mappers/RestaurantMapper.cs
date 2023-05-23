using AutoMapper;
using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Domain.Models;
using System.Collections.Generic;

namespace Foods.Application.Mappers
{
    public class RestaurantMapper
    {
        public static RestaurantResponseDTO ToRestaurantResponse(RestaurantModel restaurant)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RestaurantModel, RestaurantResponseDTO>());

            var mapper = new Mapper(config);

            return mapper.Map<RestaurantModel, RestaurantResponseDTO>(restaurant);
        }

        public static RestaurantModel ToRestaurantModel(RestaurantRequestDTO restaurant)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RestaurantRequestDTO, RestaurantModel>());

            var mapper = new Mapper(config);

            return mapper.Map<RestaurantRequestDTO, RestaurantModel>(restaurant);
        }

        public static List<ItemRestaurantResponseDTO> ToRestaurantResponse(List<ItemRestaurantModel> restaurant)
        {
            return restaurant.ConvertAll(r => new ItemRestaurantResponseDTO
            {
                Id = r.Id,
                Name = r.Name,
                UrlLogo = r.UrlLogo
            });
        }
    }
}
