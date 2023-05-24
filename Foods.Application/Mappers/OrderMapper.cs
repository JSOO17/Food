using AutoMapper;
using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Domain.Models;

namespace Foods.Application.Mappers
{
    public class OrderMapper
    {
        private static MapperConfiguration CreateConfigToResponse()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDishModel, OrderDishResponseDTO>();

                cfg.CreateMap<OrderModel, OrderResponseDTO>();
            });
        }

        private static MapperConfiguration CreateConfigToModel()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDishRequestDTO, OrderDishModel>();

                cfg.CreateMap<OrderRequestDTO, OrderModel>();
            });
        }

        public static OrderResponseDTO ToResponse(OrderModel order)
        {
            var config = CreateConfigToResponse();

            var mapper = new Mapper(config);

            return mapper.Map<OrderModel, OrderResponseDTO>(order);
        }

        public static List<OrderResponseDTO> ToResponse(List<OrderModel> order)
        {
            var config = CreateConfigToResponse();

            var mapper = new Mapper(config);

            return mapper.Map<List<OrderModel>, List<OrderResponseDTO>>(order);
        }

        public static OrderModel ToModel(OrderRequestDTO order)
        {
            var config = CreateConfigToModel();

            var mapper = new Mapper(config);

            return mapper.Map<OrderRequestDTO, OrderModel>(order);
        }

        public static OrderFiltersModel ToModel(OrderFiltersRequest filters)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderFiltersRequest, OrderFiltersModel>();
            });

            var mapper = new Mapper(config);

            return mapper.Map<OrderFiltersRequest, OrderFiltersModel>(filters);
        }
    }
}
