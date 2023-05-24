using FluentValidation;
using Foods.Domain.Models;

namespace Foods.Domain.Utils.Validators
{
    public class ValidatorOrderDishModel : AbstractValidator<OrderDishModel>
    {
        public ValidatorOrderDishModel() 
        {
            RuleFor(dish => dish.DishId)
                .NotEmpty()
                .WithMessage("The dishId can not be empty.")
                .NotNull()
                .WithMessage("The dishId can not be null.");

            RuleFor(dish => dish.Cuantity)
                .NotEmpty()
                .WithMessage("The cuantity can not be empty.")
                .NotNull()
                .WithMessage("The cuantity can not be null.");
        }
    }
}
