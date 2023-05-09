namespace Foods.Domain.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int RestaurantId { get; set; }
        public bool IsActive { get; set; }

    }
}
