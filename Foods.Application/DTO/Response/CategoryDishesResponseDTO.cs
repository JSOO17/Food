namespace Foods.Application.DTO.Response
{
    public class CategoryDishesResponseDTO
    {
        public string NameCategory { get; set; }
        public List<DishResponseDTO> Dishes { get; set; }
    }
}
