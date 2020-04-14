using Apartments.Domain.Admin.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Validation
{
    public class CommentDTOAdministrationValidator : AbstractValidator<CommentDTOAdministration>
    {
        public CommentDTOAdministrationValidator()
        {
            RuleFor(_=>_.Id).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("Id can't parse to Guid type");

            RuleFor(_ => _.Title).MinimumLength(4).MaximumLength(25)
                .WithMessage("Title must be from 4 till 25 characters");

            RuleFor(_ => _.Text).MinimumLength(4).MaximumLength(255)
                .WithMessage("Text must be from 4 till 255 characters");
        }
    }
}