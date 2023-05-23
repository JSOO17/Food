using AutoMapper;
using Foods.Domain.Models;
using Foods.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods.Infrastructure.Data.Mappers
{
    public class RestaurantMapper
    {
        public static Restaurant ToRestaurant(RestaurantModel restaurant)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RestaurantModel, Restaurant>());

            var mapper = new Mapper(config);

            return mapper.Map<RestaurantModel, Restaurant>(restaurant);
        }

        public static RestaurantModel ToRestaurantModel(Restaurant restaurant)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Restaurant, RestaurantModel>());

            var mapper = new Mapper(config);

            return mapper.Map<Restaurant, RestaurantModel>(restaurant);
        }
    }
}
