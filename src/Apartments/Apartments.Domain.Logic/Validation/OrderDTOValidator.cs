using Apartments.Domain.Users.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Validation
{
    public class OrderDTOValidator : AbstractValidator<OrderDTO>
    {
        public OrderDTOValidator()
        {
            RuleFor(_=>_.Id).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("Order Id must can parse to Guid type");

            RuleFor(_=>_.CustomerId).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("Customer Id must can parse to Guid type");

            RuleFor(_=>_.Dates).Must(_ => _.Any())
                .WithMessage("You must select rental dates");
        }
    }
}
