using FluentValidation;
using RentWheelz.API.Models;
using RentWheelz.Data.UnitOfWork;

namespace RentWheelz.API.Validators
{
    public class LoginValidator : AbstractValidator<Login>
    {
        private readonly IUnitOfWork unitOfWork;

        public LoginValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(loginDto => loginDto.UserEmail)
                .NotEmpty()
                .WithMessage("User email is required")
                .EmailAddress()
                .WithMessage("UserEmail must be a valid email address");
            //.Must(IsUserEmailExists)
            //.WithMessage("User email does not exist");

            RuleFor(loginDto => loginDto.UserPassword)
                .NotEmpty()
                .WithMessage("User password is required")
                .Length(3, 50)
                .WithMessage("User password must be between 3 and 50 characters");
        }


    }
}
