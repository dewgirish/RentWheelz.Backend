using FluentValidation;
using RentWheelz.API.Models;
using RentWheelz.Data.UnitOfWork;

namespace RentWheelz.API.Validators
{
    public class ReservationCarValidator : AbstractValidator<ReservationCar>
    {
        private readonly IUnitOfWork unitOfWork;

        public ReservationCarValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            RuleFor(reservationCar => reservationCar.pickupDate)
                .NotEmpty()
                .WithMessage("Pickup date is required")
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Pickup date must be greater than today");

            RuleFor(reservationCar => reservationCar.returnDate)
                .NotEmpty()
                .WithMessage("Return date is required")
                .GreaterThan(reservationCar => reservationCar.pickupDate)
                .WithMessage("Return date must be greater than pickup date");

            RuleFor(reservationCar => reservationCar.numOfTravellers)
                .NotEmpty()
                .WithMessage("Number of travellers is required")
                .GreaterThan(0)
                .WithMessage("Number of travellers must be greater than 0")
                .LessThanOrEqualTo(10)
                .WithMessage("Number of travellers must be less than or equal to 10");

            RuleFor(reservationCar => reservationCar.carId)
                .NotEmpty()
                .WithMessage("Car ID is required")
                .Must(CheckCarIdexists)
                .WithMessage("Car ID does not exist");
        }

        private bool CheckCarIdexists(string carId)
        {
            var car = unitOfWork.Car.Find(c => c.CarId == carId).FirstOrDefault();
            return car != null;
        }
    }
}