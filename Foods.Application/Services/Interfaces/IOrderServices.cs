using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;
using Foods.Domain.Models;

namespace Foods.Application.Services.Interfaces
{
    public interface IOrderServices
    {
        Task<OrderResponseDTO> CreateOrder(OrderRequestDTO order);
        Task<OrderResponseDTO> UpdateOrder(long id, OrderRequestDTO order, long userId, string cellphoneUser);
        Task<List<OrderResponseDTO>> GetOrders(OrderFiltersRequest filters, int page, int count, long userId);
    }
}
