using AutoMapper;
using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Domain.Models;

namespace Foods.Application.Mappers
{
    public class OrderMapper
    {
        public static OrderResponseDTO ToResponse(OrderModel order)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDishModel, OrderDishResponseDTO>();

                cfg.CreateMap<OrderModel, OrderResponseDTO>();
            });

            var mapper = new Mapper(config);

            return mapper.Map<OrderModel, OrderResponseDTO>(order);
        }

        public static OrderModel ToModel(OrderRequestDTO order)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDishRequestDTO, OrderDishModel>();

                cfg.CreateMap<OrderRequestDTO, OrderModel>();
            });

            var mapper = new Mapper(config);

            return mapper.Map<OrderRequestDTO, OrderModel>(order);
        }
    }
}
