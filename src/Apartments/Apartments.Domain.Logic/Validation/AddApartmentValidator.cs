using Apartments.Domain.Users.AddDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Validation
{
    public class AddApartmentValidator : AbstractValidator<AddApartment>
    {
        public AddApartmentValidator()
        {
            RuleFor(_ => _.Title).MinimumLength(5).MaximumLength(100)
                .WithMessage("Title must be from 5 till 100 characters");
            RuleFor(_ => _.Text).MinimumLength(10).MaximumLength(255)
                .WithMessage("Text must be from 5 till 255 characters");
            RuleFor(_ => _.Price).GreaterThan(0m)
                .WithMessage("Price must be greater than 0");
            RuleFor(_ => _.NumberOfRooms).GreaterThan(0)
                .WithMessage("Number of Rooms must be greater than 0");

            RuleFor(_ => _.Address.CountryId).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("CountryId can't parse to Guid type");
            RuleFor(_ => _.Address.City).NotEmpty()
                .WithMessage("City must not be empty");
            RuleFor(_=>_.Address.Street).NotEmpty()
                .WithMessage("Street must not be empty");
        }
    }
}
