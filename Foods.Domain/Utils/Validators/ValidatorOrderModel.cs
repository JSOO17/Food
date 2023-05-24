using FluentValidation;
using Foods.Domain.Models;

namespace Foods.Domain.Utils.Validators
{
    public class ValidatorOrderModel : AbstractValidator<OrderModel>
    {
        public ValidatorOrderModel()
        {
            RuleFor(dish => dish.State)
                .NotEmpty()
                .WithMessage("The state can not be empty.")
                .NotNull()
                .WithMessage("The state can not be null.");

            RuleFor(dish => dish.ClientId)
                .NotEmpty()
                .WithMessage("The clientId can not be empty.")
                .NotNull()
                .WithMessage("The clientId can not be null.");

            RuleFor(dish => dish.RestaurantId)
                .NotEmpty()
                .WithMessage("The restaurantId can not be empty.")
                .NotNull()
                .WithMessage("The restaurantId can not be null.");

            RuleFor(dish => dish.Date)
                .NotEmpty()
                .WithMessage("The date can not be empty.")
                .NotNull()
                .WithMessage("The date can not be null.");

            RuleForEach(x => x.Dishes).SetValidator(new ValidatorOrderDishModel());
        }
    }
}
