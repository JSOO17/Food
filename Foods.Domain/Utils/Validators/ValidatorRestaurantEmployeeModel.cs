using FluentValidation;
using Foods.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Foods.Domain.Utils.Validators
{
    public class ValidatorRestaurantEmployeeModel : AbstractValidator<RestaurantEmployeesModel>
    {
        public ValidatorRestaurantEmployeeModel()
        {
            RuleFor(restaurantEmployee => restaurantEmployee.RestaurantId)
                .NotEmpty()
                .WithMessage("The restaurantId can not be empty.")
                .NotNull()
                .WithMessage("The restaurantId can not be null.");

            RuleFor(restaurantEmployee => restaurantEmployee.EmployeeId)
                .NotEmpty()
                .WithMessage("The employeeId can not be empty.")
                .NotNull()
                .WithMessage("The employeeId can not be null.");
        }
    }
}
