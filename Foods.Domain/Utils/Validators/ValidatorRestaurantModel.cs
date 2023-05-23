using FluentValidation;
using Foods.Domain.Models;
using System.Text.RegularExpressions;

namespace Foods.Domain.Utils.Validators
{
    public class ValidatorRestaurantModel : AbstractValidator<RestaurantModel>
    {
        private const string PatternCellphone = @"^[0-9+]+$";

        public ValidatorRestaurantModel()
        {
            RuleFor(restaurant => restaurant.Name)
                .NotEmpty()
                .WithMessage("The name can not be empty.")
                .NotNull()
                .WithMessage("The name can not be null.");

            RuleFor(restaurant => restaurant.Address)
                .NotEmpty()
                .WithMessage("The address can not be empty.")
                .NotNull()
                .WithMessage("The address can not be null.");

            RuleFor(restaurant => restaurant.OwnerId)
                .NotEmpty()
                .WithMessage("The ownerId can not be empty.")
                .NotNull()
                .WithMessage("The ownerId can not be null.");

            RuleFor(restaurant => restaurant.Cellphone)
                .NotEmpty()
                .WithMessage("The cellphone can not be empty.")
                .NotNull()
                .WithMessage("The cellphone can not be null.")
                .MaximumLength(13)
                .WithMessage("The cellphone is too large.")
                .Must(cellphone => Regex.IsMatch(cellphone, PatternCellphone))
                .WithMessage("The cellphone only numbers and +");

            RuleFor(restaurant => restaurant.UrlLogo)
                .NotEmpty()
                .WithMessage("The urlLogo can not be empty.")
                .NotNull()
                .WithMessage("The urlLogo can not be null.");

            RuleFor(restaurant => restaurant.Nit)
                .NotEmpty()
                .WithMessage("The nit can not be empty.")
                .NotNull()
                .WithMessage("The nit can not be null.");
        }
    }
}
