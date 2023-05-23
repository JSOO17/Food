using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods.Domain.Exceptions
{
    public class ValidationModelException : ValidationException
    {
        public ValidationModelException(string message) : base(message)
        {
        }
    }
}
