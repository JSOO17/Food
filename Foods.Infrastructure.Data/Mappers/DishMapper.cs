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
    public class DishMapper
    {
        public static Dish ToDish(DishModel dish)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DishModel, Dish>());

            var mapper = new Mapper(config);

            return mapper.Map<DishModel, Dish>(dish);
        }

        public static DishModel ToModel(Dish dish)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Dish, DishModel>());

            var mapper = new Mapper(config);

            return mapper.Map<Dish, DishModel>(dish);
        }

        public static List<DishModel> ToModel(List<Dish> dishes)
        {
            return dishes.ConvertAll(d => new DishModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                UrlImagen = d.UrlImagen,
                CategoryId = d.CategoryId,
                RestaurantId = d.RestaurantId,
                IsActive = d.IsActive,
                Price = d.Price
            });
        }
    }
}
