using Apartments.Domain.Users.AddDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Web.Validation
{
    public class AddCommentValidator : AbstractValidator<AddComment>
    {
        public AddCommentValidator()
        {
            RuleFor(_=>_.ApartmentId).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("ApartmentId can't parse to Guid type");

            RuleFor(_=>_.Title).MinimumLength(4).MaximumLength(50)
                .WithMessage("Title must be from 4 till 50 characters");

            RuleFor(_ => _.Text).MinimumLength(4).MaximumLength(255)
                .WithMessage("Text must be from 4 till 255 characters");
        }
    }
}
