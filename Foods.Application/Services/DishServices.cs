using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Application.Mappers;
using Foods.Application.Services.Interfaces;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Interfaces.SPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods.Application.Services
{
    public class DishServices : IDishServices
    {
        private readonly IDishServicesPort _dishServicesPort;

        public DishServices(IDishServicesPort dishServicesPort)
        {
            _dishServicesPort = dishServicesPort;
        }

        public async Task<DishResponseDTO> CreateDish(DishRequestDTO dishRequest)
        {
            var dishModel = DishMapper.ToDishModel(dishRequest);

            var dish = await _dishServicesPort.CreateDish(dishModel);

            return DishMapper.ToDishResponse(dish);
        }

        public async Task<List<DishResponseDTO>> GetDishes(int page, int count, long restaurantId)
        {
            var dishes = await _dishServicesPort.GetDishes(page, count, restaurantId);

            return DishMapper.ToDishResponse(dishes);
        }

        public async Task UpdateDish(long id, DishRequestDTO dishRequest, long userId)
        {
            var dishModel = DishMapper.ToDishModel(dishRequest);

            await _dishServicesPort.UpdateDish(id, dishModel, userId);
        }
    }
}
