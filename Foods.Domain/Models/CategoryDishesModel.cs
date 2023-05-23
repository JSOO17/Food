namespace Foods.Domain.Models
{
    public class CategoryDishesModel
    {
        public string NameCategory { get; set; }
        public List<DishModel> Dishes { get; set; }
    }
}
