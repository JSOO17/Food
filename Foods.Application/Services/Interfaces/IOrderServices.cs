using Foods.Application.DTO.Request;
using Foods.Application.DTO.Response;

namespace Foods.Application.Services.Interfaces
{
    public interface IOrderServices
    {
        Task<OrderResponseDTO> CreateOrder(OrderRequestDTO order);
    }
}
