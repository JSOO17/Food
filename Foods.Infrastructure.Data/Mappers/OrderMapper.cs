using AutoMapper;
using Foods.Domain.Models;
using Foods.Infrastructure.Data.Models;

namespace Foods.Infrastructure.Data.Mappers
{
    public class OrderMapper
    {
        public static Order ToOrder(OrderModel order)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDishModel, Orderdish>();

                cfg.CreateMap<OrderModel, Order>();
            });

            var mapper = new Mapper(config);

            return mapper.Map<OrderModel, Order>(order);
        }

        public static OrderModel ToModel(Order order)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderModel>());

            var mapper = new Mapper(config);

            return mapper.Map<Order, OrderModel>(order);
        }

        public static List<Orderdish> ToOrderDishes(List<OrderDishModel> order)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDishModel, Orderdish>());

            var mapper = new Mapper(config);

            return mapper.Map<List<OrderDishModel>, List<Orderdish>>(order);
        }

        public static List<OrderDishModel> ToModel(List<Orderdish> order)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Orderdish, OrderDishModel>());

            var mapper = new Mapper(config);

            return mapper.Map<List<Orderdish>, List<OrderDishModel>>(order);
        }
    }
}
