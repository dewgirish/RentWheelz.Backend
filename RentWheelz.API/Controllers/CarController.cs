using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentWheelz.API.Models;
using RentWheelz.API.Validators;
using RentWheelz.Data.Models;
using RentWheelz.Data.UnitOfWork;

namespace RentWheelz.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ReservationCarValidator _reservationCarValidator;
        private readonly AddCarValidator _addCarValidator;

        public CarController(IUnitOfWork unitOfWork, ReservationCarValidator reservationCarValidator, AddCarValidator addCarValidator)
        {
            this._unitOfWork = unitOfWork;
            _reservationCarValidator = reservationCarValidator;
            _addCarValidator = addCarValidator;
        }

        [HttpGet("getPackages")]
        public IActionResult Get()
        {
            try
            {
                var cars = _unitOfWork.Car.GetAll();

                var returnObj = new
                {
                    status = "success",
                    results = cars.Count(),
                    data = new
                    {
                        cars = cars
                    }
                };

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        [HttpPost("addCar")]
        [Authorize]
        public IActionResult AddCar([FromBody] AddCar pCar)
        {
            try
            {
                var validationResult = _addCarValidator.Validate(pCar);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new MyCustomHttpResponse(false, validationResult.Errors[0].ErrorMessage));
                }

                var car = new Car
                {
                    CarId = Guid.NewGuid().ToString("N").Substring(0, 6),
                    CarModel = pCar.CarModel,
                    RegistrationNumber = pCar.RegistrationNumber,
                    CarAvailability = pCar.CarAvailability ? "YES" : "NO",
                    Brand = pCar.Brand,
                    PricePerHour = pCar.PricePerHour,
                    Thumbnail = pCar.Thumbnail
                };

                _unitOfWork.Car.Add(car);
                _unitOfWork.Save();

                return Ok(new
                {
                    status = "success",
                    message = "Car added successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        [HttpPost("reserve")]
        [Authorize]
        public IActionResult Reserve([FromBody] ReservationCar pCar)
        {
            try
            {
                var result = _reservationCarValidator.Validate(pCar);

                if (!result.IsValid)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = result.Errors[0].ErrorMessage
                    });
                }

                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserEmail").Value;

                var myCar = _unitOfWork.Car.Find(x => x.CarId == pCar.carId).FirstOrDefault();

                //check mycar is not null
                if (myCar == null)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Car not found"
                    });
                }

                var days = pCar.returnDate.DayNumber - pCar.pickupDate.DayNumber;
                var total = days * myCar.PricePerHour;

                //check if car is available between the pickup and return date
                var bookedCar = _unitOfWork.Reservation
                    .Find(x => x.CarId == pCar.carId && x.Status == "CONFIRMED");

                var currentBooking = bookedCar.Where(b => (pCar.pickupDate <= b.ReturnDate && pCar.pickupDate >= b.PickupDate
                || b.PickupDate <= pCar.returnDate && b.PickupDate >= pCar.pickupDate)).FirstOrDefault();

                if (currentBooking  != null)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Car is not available for the selected dates"
                    });
                }

                var bookingId = Guid.NewGuid().ToString("N").Substring(0, 6);

                //save reservation to database
                var reservation = new Reservation
                {
                    BookingId = bookingId,
                    CarId = pCar.carId,
                    UserEmail = currentUserEmail,
                    ReservationDate = DateOnly.FromDateTime(DateTime.Now),
                    PickupDate = pCar.pickupDate,
                    ReturnDate = pCar.returnDate,
                    NumOfTravellers = pCar.numOfTravellers,
                    Status = "CONFIRMED",
                    Car = myCar.Brand + " " + myCar.CarModel,
                    Img = myCar.Thumbnail,
                    Total = total
                };

                _unitOfWork.Reservation.Add(reservation);

                _unitOfWork.Save();
                //return booking id

                return Ok(new
                {
                    status = "success",
                    message = "Reservation successful",
                    data = new
                    {
                        bookingId = bookingId,
                        userEmail = currentUserEmail
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        [HttpPost("my-bookings")]
        [Authorize]
        public IActionResult MyBookings(string yourEmailId)
        {
            try
            {
                if (string.IsNullOrEmpty(yourEmailId))
                {
                    return BadRequest(new MyCustomHttpResponse(false, "User email is required"));
                }

                string currentUserEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserEmail").Value;

                if (string.Compare(currentUserEmail, yourEmailId, true) != 0)
                {
                    return BadRequest(new MyCustomHttpResponse(false, "You can not view the Other Users Bookings"));
                }

                var myBookings = _unitOfWork.Reservation.Find(x => x.UserEmail == currentUserEmail).ToList();

                return Ok(new
                {
                    status = "success",
                    data = new
                    {
                        bookings = myBookings
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        [HttpPost("cancel-booking")]
        [Authorize]
        public IActionResult CancelBooking(string bookingId)
        {
            try
            {
                if (string.IsNullOrEmpty(bookingId))
                {
                    return BadRequest(new MyCustomHttpResponse(false, "Booking id is required"));
                }

                var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserEmail").Value;

                var myBooking = _unitOfWork.Reservation.Find(x => x.BookingId == bookingId).FirstOrDefault();

                if (myBooking == null)
                {
                    return BadRequest(new MyCustomHttpResponse(false, "Booking not found"));
                }

                if (myBooking.UserEmail != currentUserEmail)
                {
                    return BadRequest(new MyCustomHttpResponse(false, "You can not cancel other users bookings"));
                }

                myBooking.Status = "CANCELLED";

                _unitOfWork.Save();

                return Ok(new
                {
                    status = "success",
                    message = "Your reservation is canceled"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }
    }
}