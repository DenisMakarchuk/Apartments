using Apartments.Domain.Users.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Web.Validation
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(_=>_.Id).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("User Id must can parse to Guid type");

            RuleFor(_ => _.Name).NotEmpty().WithMessage("You must write the name");
        }
    }
}
