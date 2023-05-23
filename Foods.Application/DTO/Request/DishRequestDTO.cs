namespace Foods.Application.DTO.Request
{
    public class DishRequestDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? UrlImagen { get; set; }
        public decimal? Price { get; set; }
        public long? CategoryId { get; set; }
        public long? RestaurantId { get; set; }
        public bool? IsActive { get; set; }
    }
}
