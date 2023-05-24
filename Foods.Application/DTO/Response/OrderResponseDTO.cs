using Foods.Domain.Models;

namespace Foods.Application.DTO.Response
{
    public class OrderResponseDTO
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public DateTime Date { get; set; }
        public string State { get; set; } = null!;
        public long ChefId { get; set; }
        public long RestaurantId { get; set; }
        public List<OrderDishResponseDTO> Dishes { get; set; }
    }
}
