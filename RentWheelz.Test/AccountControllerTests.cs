using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using RentWheelz.API.Controllers;
using RentWheelz.API.Helper;
using RentWheelz.API.Models;
using RentWheelz.API.Validators;
using RentWheelz.Data.Models;
using RentWheelz.Data.Repository;
using RentWheelz.Data.UnitOfWork;

namespace RentWheelz.API.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<IUserRepository> _userRepository;
        private AccountController _accountController;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private RegisterUserValidator _registerUserValidatorMock;

        //private Mock<LoginValidator> _loginValidatorMock;
        private Mock<TokenGenerator> _tokenGeneratorMock;

        private IConfiguration _configuration;

        private LoginValidator _loginValidator;

        [TestInitialize]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string> {
             {"JwtSettings:SecretKey", "ThisIsMyLongAndSecureSecretKey1234567890abcdefgh"},
             {"JwtSettings:Issuer", "test_token"},
             {"JwtSettings:Audience", "test_token"},
            {"JwtSettings:DurationInMinutes","4" }
         };

            _configuration= new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _userRepository = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _registerUserValidatorMock = new RegisterUserValidator(_unitOfWorkMock.Object);
            _loginValidator =  new LoginValidator(_unitOfWorkMock.Object);
            _tokenGeneratorMock = new Mock<TokenGenerator>(_configuration);

            _accountController = new AccountController(
                _unitOfWorkMock.Object,
                _registerUserValidatorMock,
                _loginValidator,
                _tokenGeneratorMock.Object);
        }

        [TestMethod]
        public void Post_WithValidLogin_ReturnsOkResultWithToken()
        {
            // Arrange
            var login = new Login
            {
                UserEmail = "test@example.com",
                UserPassword = "password"
            };

            var user = new User
            {
                UserName = "Test User",
                UserEmail = "test@example.com",
                UserPassword = "password",
                ProofId = "1234567890"
            };
            // setup the user repository here _userRepository
            _userRepository.Setup(x => x.Find(x => x.UserEmail == login.UserEmail)).Returns(new List<User> { user });

            //setup the user mock

            _unitOfWorkMock.Setup(x => x.User.Find(x => x.UserEmail == login.UserEmail
            && x.UserPassword == login.UserPassword))
                .Returns(new List<User> { user });

            // Act
            var result = _accountController.Post(login) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("success", result.Value.GetType().GetProperty("status").GetValue(result.Value));
            Assert.AreEqual("Login successful", result.Value.GetType().GetProperty("message").GetValue(result.Value));
            Assert.IsNotNull(result.Value.GetType().GetProperty("data").GetValue(result.Value));
            Assert.IsNotNull(result.Value.GetType().GetProperty("Authorization").GetValue(result.Value));
        }

        [TestMethod]
        public void Post_WithInvalidLogin_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var login = new Login
            {
                UserEmail = "",
                UserPassword = "password"
            };

            // Act
            var result = _accountController.Post(login) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("User email is required", ((MyCustomHttpResponse)(result.Value)).Message);
        }

        [TestMethod]
        public void Post_WithInvalidUser_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var login = new Login
            {
                UserEmail = "test@example.com",
                UserPassword = "password"
            };

            _unitOfWorkMock.Setup(x => x.User.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .Returns(new List<User>());

            // Act
            var result = _accountController.Post(login) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid User/Password", ((MyCustomHttpResponse)(result.Value)).Message);
        }

        [TestMethod]
        public void Register_WithValidRegisterUser_ReturnsOkResult()
        {
            // Arrange

            var registerUser = new RegisterUser
            {
                UserEmail = new Bogus.Faker().Internet.Email(),
                UserName = new Bogus.Faker().Internet.UserName(),
                UserPassword = new Bogus.Faker().Internet.Password(),
                ProofId = Convert.ToString(new Bogus.Faker().Address.Random),
            };

            _unitOfWorkMock.Setup(x => x.User.Add(It.IsAny<User>()));
            _unitOfWorkMock.Setup(x => x.Save()).Returns(1);

            // Act
            var result = _accountController.Register(registerUser) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("success", result.Value.GetType().GetProperty("status").GetValue(result.Value));
            Assert.AreEqual("User registered successfully", result.Value.GetType().GetProperty("message").GetValue(result.Value));
        }

        [TestMethod]
        public void Register_WithInvalidRegisterUser_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var registerUser = new RegisterUser
            {
                UserEmail = new Bogus.Faker().Internet.Email(),
                UserName = new Bogus.Faker().Internet.UserName(),

                ProofId = Convert.ToString(new Bogus.Faker().Address.Random),
            };

            // setup the user repository here unit of work
            _unitOfWorkMock.Setup(x => x.User.Add(It.IsAny<User>()));
            _unitOfWorkMock.Setup(x => x.Save()).Returns(1);

            // Act
            var result = _accountController.Register(registerUser) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("User password is required", ((MyCustomHttpResponse)(result.Value)).Message);
        }
    }
}