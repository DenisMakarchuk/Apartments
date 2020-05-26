using Apartments.Domain.Users.AddDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Validation
{
    public class AddOrderValidator : AbstractValidator<AddOrder>
    {
        public AddOrderValidator()
        {
            RuleFor(_=>_.ApartmentId).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("ApartmentId can't parse to Guid type");

            RuleFor(_ => _.Dates).Must(_=>_ != null).Must(_ => _.Any())
                .WithMessage("You must select rental dates");
        }
    }
}