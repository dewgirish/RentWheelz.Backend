using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RentWheelz.API.Controllers;
using RentWheelz.API.Models;
using RentWheelz.API.Validators;
using RentWheelz.Data.Models;
using RentWheelz.Data.UnitOfWork;
using System.Linq.Expressions;
using System.Security.Claims;

namespace RentWheelz.API.Tests.Controllers
{
    [TestClass]
    public class CarControllerTests
    {
        private CarController _carController;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private ReservationCarValidator _reservationCarValidator;
        private AddCarValidator _addCarValidator;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _reservationCarValidator = new ReservationCarValidator(_unitOfWorkMock.Object);
            _addCarValidator = new AddCarValidator();

            _carController = new CarController(_unitOfWorkMock.Object, _reservationCarValidator, _addCarValidator);
        }

        [TestMethod]
        public void AddCar_ValidCar_ReturnsOkResult()
        {
            // Arrange
            var pCar = new AddCar
            {
                CarModel = new Faker().Vehicle.Model(),
                RegistrationNumber = new Faker().Random.AlphaNumeric(6).ToUpper(),
                CarAvailability = true,
                Brand = new Faker().Company.CompanyName(),
                PricePerHour = new Faker().Random.Int(1, 100),
                Thumbnail = new Faker().Image.PicsumUrl()
            };

            _unitOfWorkMock.Setup(u => u.Car.Add(It.IsAny<Car>()));
            _unitOfWorkMock.Setup(u => u.Save());

            // Act
            var result = _carController.AddCar(pCar);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual("success", okResult.Value.GetType().GetProperty("status").GetValue(okResult.Value));
            Assert.AreEqual("Car added successfully", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
        }

        [TestMethod]
        public void AddCar_InvalidCar_ReturnsBadRequest()
        {
            // Arrange
            var pCar = new AddCar
            {
                CarModel = "",
                RegistrationNumber = new Faker().Random.AlphaNumeric(6).ToUpper(),
                CarAvailability = true,
                Brand = new Faker().Company.CompanyName(),
                PricePerHour = new Faker().Random.Int(1, 100),
                Thumbnail = new Faker().Image.PicsumUrl()
            };
            // Act
            var result = _carController.AddCar(pCar);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
            var badRequestResult = ((BadRequestObjectResult)result).Value as MyCustomHttpResponse;
            Assert.AreEqual("Fail", badRequestResult.Status);
            Assert.AreEqual("Car model is required", badRequestResult.Message);
        }

        [TestMethod]
        public void Reserve_ValidReservation_ReturnsOkResult()
        {
            // Arrange

            var pCar = new ReservationCar
            {
                carId = new Faker().Random.AlphaNumeric(6),
                pickupDate = DateOnly.FromDateTime(new Faker().Date.Recent().AddDays(2)),
                returnDate =  DateOnly.FromDateTime(new Faker().Date.Recent().AddDays(3)),
                numOfTravellers = new Faker().Random.Number(1, 10)
            };

            var currentUserEmail = "test@example.com";

            var carFaker = new Faker<Car>()
                .RuleFor(c => c.CarId, f => f.Random.AlphaNumeric(6))
                .RuleFor(c => c.CarModel, f => f.Vehicle.Model())
                .RuleFor(c => c.RegistrationNumber, f => f.Random.AlphaNumeric(6).ToUpper())
                .RuleFor(c => c.CarAvailability, f => "YES")
                .RuleFor(c => c.Brand, f => f.Company.CompanyName())
                .RuleFor(c => c.PricePerHour, f => f.Random.Int(1, 100))
                .RuleFor(c => c.Thumbnail, f => f.Image.PicsumUrl());

            var car = carFaker.Generate();

            _unitOfWorkMock.Setup(u => u.Car.Find(It.IsAny<Expression<Func<Car, bool>>>())).Returns(new List<Car> { car });
            _unitOfWorkMock.Setup(u => u.Reservation.Find(It.IsAny<Expression<Func<Reservation, bool>>>())).Returns(new List<Reservation>());
            _unitOfWorkMock.Setup(u => u.Reservation.Add(It.IsAny<Reservation>()));
            _unitOfWorkMock.Setup(u => u.Save());

            _carController.ControllerContext = new ControllerContext();
            _carController.ControllerContext.HttpContext = new DefaultHttpContext();
            _carController.ControllerContext.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmail", currentUserEmail)
            }, "mock"));

            // Act
            var result = _carController.Reserve(pCar);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual("success", okResult.Value.GetType().GetProperty("status").GetValue(okResult.Value));
            Assert.AreEqual("Reservation successful", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
            Assert.IsNotNull(okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value).GetType().GetProperty("bookingId").GetValue(okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value)));
            Assert.AreEqual(currentUserEmail, okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value).GetType().GetProperty("userEmail").GetValue(okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value)));
        }

        [TestMethod]
        public void Reserve_InvalidReservation_ReturnsBadRequest()
        {
            // Arrange

            var pCar = new ReservationCar
            {
                carId = "123456",
                pickupDate = DateOnly.FromDateTime(new Faker().Date.Recent().AddDays(2)),
                returnDate =  DateOnly.FromDateTime(new Faker().Date.Recent().AddDays(3)),
                numOfTravellers = new Faker().Random.Number(1, 10)
            };

            var currentUserEmail = "test@example.com";

            var car = new Car
            {
                CarId = "123456",
                CarModel = "Test Car",
                RegistrationNumber = "ABC123",
                CarAvailability = "YES",
                Brand = "Test Brand",
                PricePerHour = 10,
                Thumbnail = "test.jpg"
            };

            var existingReservation = new Reservation
            {
                CarId = "123456",
                PickupDate = DateOnly.FromDateTime(new Faker().Date.Recent(1)),
                ReturnDate = DateOnly.FromDateTime(new Faker().Date.Recent().AddDays(4)),
                Status = "CONFIRMED"
            };

            _carController.ControllerContext = new ControllerContext();
            _carController.ControllerContext.HttpContext = new DefaultHttpContext();
            _carController.ControllerContext.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmail", currentUserEmail)
            }, "mock"));

            _unitOfWorkMock.Setup(u => u.Car.Find(It.IsAny<Expression<Func<Car, bool>>>())).Returns(new List<Car> { car });
            _unitOfWorkMock.Setup(u => u.Reservation.Find(It.IsAny<Expression<Func<Reservation, bool>>>())).Returns(new List<Reservation> { existingReservation });

            // Act
            var result = _carController.Reserve(pCar);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("error", badRequestResult.Value.GetType().GetProperty("status").GetValue(badRequestResult.Value));
            Assert.AreEqual("Car is not available for the selected dates", badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value));
        }

        [TestMethod]
        public void MyBookings_ValidEmail_ReturnsOkResult()
        {
            // Arrange
            var yourEmailId = "test@example.com";

            var currentUserEmail = "test@example.com";

            var myBookings = new List<Reservation>
            {
                new Reservation
                {
                    BookingId = "123456",
                    CarId = "789012",
                    UserEmail = currentUserEmail,
                    ReservationDate = DateOnly.FromDateTime(DateTime.Now),
                    PickupDate =  DateOnly.FromDateTime(new Faker().Date.Recent().AddDays(0)),
                    ReturnDate =  DateOnly.FromDateTime(new Faker().Date.Recent().AddDays(2)),
                    NumOfTravellers = 2,
                    Status = "CONFIRMED",
                    Car = "Test Brand Test Car",
                    Img = "test.jpg",
                    Total = 20.0
                }
            };

            _unitOfWorkMock.Setup(u => u.Reservation.Find(It.IsAny<Expression<Func<Reservation, bool>>>())).Returns(myBookings);

            _carController.ControllerContext = new ControllerContext();
            _carController.ControllerContext.HttpContext = new DefaultHttpContext();
            _carController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmail", currentUserEmail)
            }, "mock"));

            // Act
            var result = _carController.MyBookings(yourEmailId);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var data = (okResult.Value.GetType().GetProperty("data").GetValue(okResult.Value));
            var bookigns = data.GetType().GetProperty("bookings").GetValue(data) as IList<Reservation>;
            Assert.AreEqual("success", okResult.Value.GetType().GetProperty("status").GetValue(okResult.Value));
            Assert.AreEqual(myBookings.Count, bookigns.Count);
            Assert.AreEqual(myBookings.First().BookingId, bookigns.First().BookingId);
        }

        [TestMethod]
        public void MyBookings_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var yourEmailId = "";

            // Act
            var result = _carController.MyBookings(yourEmailId);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);

            var badRequestResult = ((BadRequestObjectResult)result).Value as MyCustomHttpResponse;
            Assert.AreEqual("Fail", badRequestResult.Status);
            Assert.AreEqual("User email is required", badRequestResult.Message);
        }

        [TestMethod]
        public void MyBookings_UnauthorizedUser_ReturnsBadRequest()
        {
            // Arrange
            var yourEmailId = "test@example.com";

            var currentUserEmail = "other@example.com";

            _carController.ControllerContext = new ControllerContext();
            _carController.ControllerContext.HttpContext = new DefaultHttpContext();
            _carController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmail", currentUserEmail)
            }, "mock"));

            // Act
            var result = _carController.MyBookings(yourEmailId);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);

            var badRequestResult = ((BadRequestObjectResult)result).Value as MyCustomHttpResponse;

            Assert.AreEqual("Fail", badRequestResult.Status);
            Assert.AreEqual("You can not view the Other Users Bookings", badRequestResult.Message);
        }

        [TestMethod]
        public void CancelBooking_ValidBookingId_ReturnsOkResult()
        {
            // Arrange
            var bookingId = "123456";

            var currentUserEmail = "test@example.com";

            var myBooking = new Reservation
            {
                BookingId = bookingId,
                UserEmail = currentUserEmail
            };

            _unitOfWorkMock.Setup(u => u.Reservation.Find(It.IsAny<Expression<Func<Reservation, bool>>>())).Returns(new List<Reservation> { myBooking });
            _unitOfWorkMock.Setup(u => u.Save());

            _carController.ControllerContext = new ControllerContext();
            _carController.ControllerContext.HttpContext = new DefaultHttpContext();
            _carController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmail", currentUserEmail)
            }, "mock"));

            // Act
            var result = _carController.CancelBooking(bookingId);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual("success", okResult.Value.GetType().GetProperty("status").GetValue(okResult.Value));
            Assert.AreEqual("Your reservation is canceled", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
        }

        [TestMethod]
        public void CancelBooking_InvalidBookingId_ReturnsBadRequest()
        {
            // Arrange
            var bookingId = "";

            // Act
            var result = _carController.CancelBooking(bookingId);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
            // var badRequestResult = (BadRequestObjectResult)result;
            var badRequestResult = ((BadRequestObjectResult)result).Value as MyCustomHttpResponse;
            Assert.AreEqual("Fail", badRequestResult.Status);
            Assert.AreEqual("Booking id is required", badRequestResult.Message);
        }

        [TestMethod]
        public void CancelBooking_UnauthorizedUser_ReturnsBadRequest()
        {
            // Arrange
            var bookingId = "123456";

            var currentUserEmail = "other@example.com";

            _carController.ControllerContext = new ControllerContext();
            _carController.ControllerContext.HttpContext = new DefaultHttpContext();
            _carController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmail", currentUserEmail)
            }, "mock"));

            var myBooking = new Reservation
            {
                BookingId = bookingId,
                UserEmail = "test@example.com"
            };

            _unitOfWorkMock.Setup(u => u.Reservation.Find(It.IsAny<Expression<Func<Reservation, bool>>>())).Returns(new List<Reservation> { myBooking });
            _unitOfWorkMock.Setup(u => u.Save());

            // Act
            var result = _carController.CancelBooking(bookingId);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);

            var badRequestResult = ((BadRequestObjectResult)result).Value as MyCustomHttpResponse;
            Assert.AreEqual("Fail", badRequestResult.Status);
            Assert.AreEqual("You can not cancel other users bookings", badRequestResult.Message);
        }
    }
}