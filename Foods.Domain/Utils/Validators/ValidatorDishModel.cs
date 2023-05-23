using FluentValidation;
using Foods.Domain.Models;

namespace Foods.Domain.Utils.Validators
{
    public class ValidatorDishModel : AbstractValidator<DishModel>
    {
        public ValidatorDishModel() 
        {
            RuleFor(dish => dish.Name)
                .NotEmpty()
                .WithMessage("The name can not be empty.")
                .NotNull()
                .WithMessage("The name can not be null.");

            RuleFor(dish => dish.Description)
                .NotEmpty()
                .WithMessage("The description can not be empty.")
                .NotNull()
                .WithMessage("The description can not be null.");

            RuleFor(dish => dish.UrlImagen)
                .NotEmpty()
                .WithMessage("The urlImagen can not be empty.")
                .NotNull()
                .WithMessage("The urlImagen can not be null.");

            RuleFor(dish => dish.Price)
                .NotEmpty()
                .WithMessage("The price can not be empty.")
                .Must(dish => dish >  0)
                .WithMessage("The price must be positive")
                .NotNull()
                .WithMessage("The price can not be null.");

            RuleFor(dish => dish.CategoryId)
                .NotEmpty()
                .WithMessage("The categoryId can not be empty.")
                .NotNull()
                .WithMessage("The categoryId can not be null.");

            RuleFor(dish => dish.RestaurantId)
                .NotEmpty()
                .WithMessage("The restaurantId can not be empty.")
                .NotNull()
                .WithMessage("The restaurantId can not be null.");

            RuleFor(dish => dish.IsActive)
                .NotEmpty()
                .WithMessage("The isActive can not be empty.")
                .NotNull()
                .WithMessage("The isActive can not be null.");
        }
    }
}
