// Ignore Spelling: Validator

using FluentValidation;
using RentWheelz.API.Models;
using RentWheelz.Data.Models;
using RentWheelz.Data.UnitOfWork;

namespace RentWheelz.API.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        // validate #User model using fluent validation library
        private readonly IUnitOfWork unitOfWork;

        public RegisterUserValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(user => user.UserName)
                .NotEmpty()
                .WithMessage("User name is required")
                .Length(3, 50)
                .WithMessage("User name must be between 3 and 50 characters");

            RuleFor(user => user.UserEmail)
                .NotEmpty().WithMessage("User email is required")
                .EmailAddress()
                .WithMessage("UserEmail must be a valid email address")
                .Must(IsUniqueUserEmail)
                .WithMessage("User email already exists");

            RuleFor(user => user.UserPassword)
                .NotEmpty()
                .WithMessage("User password is required")
                .Length(3, 50)
                .WithMessage("User password must be between 3 and 50 characters");

            RuleFor(user => user.ProofId)
                .NotEmpty()
                .WithMessage("Proof id is required")
                .Length(3, 50)
                .WithMessage("Proof id must be between 3 and 50 characters");
        }

        // check user email is unique or not
        private bool IsUniqueUserEmail(string userEmail)
        {
            var user = unitOfWork.User.Find(u => u.UserEmail == userEmail).FirstOrDefault();
            return user == null;
        }
    }
}