using Apartments.Domain.Users.AddDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Web.Validation
{
    public class AddUserValidator : AbstractValidator<AddUser>
    {
        public AddUserValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
        }
    }
}
