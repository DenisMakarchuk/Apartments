using Apartments.Domain.Users.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apartments.Domain.Logic.Validation
{
    public class ApartmentViewValidator : AbstractValidator<ApartmentView>
    {
        public ApartmentViewValidator()
        {
            RuleFor(_ => _.Country).NotNull()
                .WithMessage("You must choose the country");

            RuleFor(_ => _.Apartment).NotNull()
                .WithMessage("Apartment must not be NULL")
                .Must(apartment=> Guid.TryParse(apartment.Id, out var _))
                .WithMessage("Apartment Id must can parse to Guid type");

            RuleFor(_ => _.Apartment.Title).MinimumLength(5).MaximumLength(120)
                .WithMessage("Title must be from 5 till 120 characters");
            RuleFor(_ => _.Apartment.Text).MinimumLength(5).MaximumLength(500)
                .WithMessage("Text must be from 5 till 500 characters");
            RuleFor(_ => _.Apartment.Price).GreaterThan(0m)
                .WithMessage("Price must be greater than 0");
            RuleFor(_ => _.Apartment.NumberOfRooms).GreaterThan(0)
                .WithMessage("Number of Rooms must be greater than 0");

            RuleFor(_ => _.Address.CountryId).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("CountryId must can parse to Guid type");
            RuleFor(_=>_.Address.Id).Must(id => Guid.TryParse(id, out var _))
                .WithMessage("Address Id must can parse to Guid type");
            RuleFor(_ => _.Address.City).NotEmpty()
                .WithMessage("City must not be empty");
            RuleFor(_ => _.Address.Street).NotEmpty()
                .WithMessage("Street must not be empty");
        }
    }
}
