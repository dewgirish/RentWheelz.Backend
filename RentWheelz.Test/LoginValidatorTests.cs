using FluentValidation.TestHelper;
using Moq;
using RentWheelz.API.Models;
using RentWheelz.API.Validators;
using RentWheelz.Data.Models;
using RentWheelz.Data.UnitOfWork;
using System.Linq.Expressions;

namespace RentWheelz.API.Tests.Validators
{
    [TestClass]
    public class LoginValidatorTests
    {
        private readonly LoginValidator validator;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        public LoginValidatorTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            validator = new LoginValidator(unitOfWorkMock.Object);
        }

        [TestMethod]
        public void ShouldHaveError_WhenUserEmailIsEmpty()
        {
            var login = new Login { UserEmail = "" };

            var result = validator.TestValidate(login);

            result.ShouldHaveValidationErrorFor(l => l.UserEmail)
                .WithErrorMessage("User email is required");
        }

        [TestMethod]
        public void ShouldHaveError_WhenUserEmailIsInvalid()
        {
            var login = new Login { UserEmail = "invalidemail" };

            var result = validator.TestValidate(login);

            result.ShouldHaveValidationErrorFor(l => l.UserEmail)
                .WithErrorMessage("UserEmail must be a valid email address");
        }

        
        [TestMethod]
        public void ShouldHaveError_WhenUserPasswordIsEmpty()
        {
            var login = new Login { UserPassword = "" };

            var result = validator.TestValidate(login);

            result.ShouldHaveValidationErrorFor(l => l.UserPassword)
                .WithErrorMessage("User password is required");
        }

        [TestMethod]
        public void ShouldHaveError_WhenUserPasswordIsTooShort()
        {
            var login = new Login { UserPassword = "12" };

            var result = validator.TestValidate(login);

            result.ShouldHaveValidationErrorFor(l => l.UserPassword)
                .WithErrorMessage("User password must be between 3 and 50 characters");
        }

        [TestMethod]
        public void ShouldHaveError_WhenUserPasswordIsTooLong()
        {
            var login = new Login { UserPassword = "123456789012345678901234567890123456789012345678901" };

            var result = validator.TestValidate(login);

            result.ShouldHaveValidationErrorFor(l => l.UserPassword)
                .WithErrorMessage("User password must be between 3 and 50 characters");
        }

        [TestMethod]
        public void ShouldNotHaveError_WhenUserEmailAndPasswordAreValid()
        {
            var login = new Login { UserEmail = "valid@example.com", UserPassword = "password" };
            unitOfWorkMock.Setup(u => u.User.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User> { new User() });

            var result = validator.TestValidate(login);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}