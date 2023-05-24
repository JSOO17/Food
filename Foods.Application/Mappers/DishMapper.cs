using AutoMapper;
using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Domain.Models;
using System.Collections.Generic;

namespace Foods.Application.Mappers
{
    public class DishMapper
    {
        public static DishResponseDTO ToDishResponse(DishModel user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DishModel, DishResponseDTO>());

            var mapper = new Mapper(config);
            
            return mapper.Map<DishModel, DishResponseDTO>(user);
        }

        public static List<DishResponseDTO> ToDishResponse(List<DishModel> dishes)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DishModel, DishResponseDTO>());

            var mapper = new Mapper(config);
            
            return mapper.Map<List<DishModel>, List<DishResponseDTO>>(dishes);
        }

        public static DishModel ToDishModel(DishRequestDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DishRequestDTO, DishModel>());

            var mapper = new Mapper(config);

            return mapper.Map<DishRequestDTO, DishModel>(user);
        }

        public static List<CategoryDishesResponseDTO> ToDishResponse(List<CategoryDishesModel> dishes)
        {
            return dishes.ConvertAll(d => new CategoryDishesResponseDTO
            {
                NameCategory = d.NameCategory,
                Dishes = d.Dishes.ConvertAll(dd => new DishResponseDTO
                {
                    Id = dd.Id,
                    Name = dd.Name,
                    Description = dd.Description,
                    UrlImagen = dd.UrlImagen,
                    CategoryId = dd.CategoryId,
                    RestaurantId = dd.RestaurantId,
                    IsActive = dd.IsActive,
                    Price = dd.Price
                })
            });
        }
    }
}
