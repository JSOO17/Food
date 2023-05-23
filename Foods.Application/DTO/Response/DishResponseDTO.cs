namespace Foods.Application.DTO.Response
{
    public class DishResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlImagen { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
        public long RestaurantId { get; set; }
        public bool IsActive { get; set; }
    }
}
