namespace Foods.Application.DTO.Request
{
    public class OrderRequestDTO
    {
        public long? Id { get; set; }
        public long? ClientId { get; set; }
        public DateTime? Date { get; set; }
        public string? State { get; set; } = null!;
        public string? Code { get; set; } = null!;
        public long? ChefId { get; set; }
        public long? RestaurantId { get; set; }
        public List<OrderDishRequestDTO>? Dishes { get; set; }
    }
}
