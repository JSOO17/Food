using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Application.Mappers;
using Foods.Application.Services.Interfaces;
using Foods.Domain.Interfaces.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods.Application.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderServicesPort _orderServicesPort;

        public OrderServices(IOrderServicesPort orderServicesPort)
        {
            _orderServicesPort = orderServicesPort;
        }

        public async Task<OrderResponseDTO> CreateOrder(OrderRequestDTO orderRequest)
        {
            var model = OrderMapper.ToModel(orderRequest);

            var order = await _orderServicesPort.CreateOrder(model);

            return OrderMapper.ToResponse(order);
        }

        public async Task<List<OrderResponseDTO>> GetOrders(OrderFiltersRequest filters, int page, int count, long userId)
        {
            var filtersModel = OrderMapper.ToModel(filters);

            var orders = await _orderServicesPort.GetOrders(filtersModel, page, count, userId);

            return OrderMapper.ToResponse(orders);
        }
    }
}
