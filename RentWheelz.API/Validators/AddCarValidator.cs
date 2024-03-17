using FluentValidation;
using RentWheelz.API.Models;

namespace RentWheelz.API.Validators
{
    public class AddCarValidator : AbstractValidator<AddCar>
    {
        //validate the car model

        public AddCarValidator()
        {
            RuleFor(addCar => addCar.CarModel)
                .NotEmpty()
                .WithMessage("Car model is required")
                .MinimumLength(3)
                .WithMessage("Car model must be at least 3 characters long")
                .MaximumLength(50)
                .WithMessage("Car model must be at most 50 characters long");

            RuleFor(addCar => addCar.RegistrationNumber)
                .NotEmpty()
                .WithMessage("Registration number is required")
                .MinimumLength(3)
                .WithMessage("Registration number must be at least 3 characters long")
                .MaximumLength(50)
                .WithMessage("Registration number must be at most 50 characters long");

            RuleFor(addCar => addCar.CarAvailability)
                .NotEmpty()
                .WithMessage("Car availability is required");

            RuleFor(addCar => addCar.Brand)
                .NotEmpty()
                .WithMessage("Brand is required")
                .MinimumLength(3)
                .WithMessage("Brand must be at least 3 characters long")
                .MaximumLength(50)
                .WithMessage("Brand must be at most 50 characters long");

            RuleFor(addCar => addCar.PricePerHour)
                .NotEmpty()
                .WithMessage("Price per hour is required")
                .GreaterThan(0)
                .WithMessage("Price per hour must be greater than 0");

            RuleFor(addCar => addCar.Thumbnail)
                .NotEmpty()
                .WithMessage("Thumbnail is required")
                .MinimumLength(3)
                .WithMessage("Thumbnail must be at least 10 characters long")
                .MaximumLength(5000)
                .WithMessage("Thumbnail must be at most 5000 characters long");
        }
    }
}