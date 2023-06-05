namespace Foods.Domain.Models
{
    public class OrderModel
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public DateTime Date { get; set; }
        public string State { get; set; } = null!;
        public long ChefId { get; set; }
        public long RestaurantId { get; set; }
        public string Code { get; set; }
        public List<OrderDishModel> Dishes { get; set; }
    }
}
