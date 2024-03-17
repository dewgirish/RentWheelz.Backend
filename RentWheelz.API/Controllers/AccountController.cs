// Ignore Spelling: Validator

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentWheelz.API.Helper;
using RentWheelz.API.Models;
using RentWheelz.API.Validators;
using RentWheelz.Data.Models;
using RentWheelz.Data.UnitOfWork;

namespace RentWheelz.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly RegisterUserValidator _registerUserValidator;

        private readonly LoginValidator _loginValidator;
        private TokenGenerator _tokenGenerator;

        public AccountController(
            IUnitOfWork unitOfWork,
            RegisterUserValidator _registerUserValidator,
            LoginValidator loginValidator,
            TokenGenerator tokenGenerator)
        {
            this._unitOfWork = unitOfWork;
            this._registerUserValidator = _registerUserValidator;
            this._loginValidator = loginValidator;
            this._tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Post([FromBody] Login login)
        {
            var validationResult = _loginValidator.Validate(login);

            if (!validationResult.IsValid)
            {
                return BadRequest(new MyCustomHttpResponse(false, validationResult.Errors[0].ErrorMessage));
            }

            var myuser = _unitOfWork.User.Find(x => x.UserEmail == login.UserEmail
            && x.UserPassword == login.UserPassword).FirstOrDefault();

            if (myuser == null || myuser.UserEmail.Length <= 0)
            {
                return BadRequest(new MyCustomHttpResponse(false, "Invalid User/Password"));
            }

            var token = _tokenGenerator.GenerateToken(myuser, new List<string> { "User" });

            return Ok(new
            {
                status = "success",
                message = "Login successful",
                data = new
                {
                    userName = myuser.UserName,
                    userEmail = myuser.UserEmail,
                    proofId = myuser.ProofId
                },
                Authorization = "Bearer " + token
            });
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterUser registerUser)
        {
            //valdate RegisterUser model using fluent validation library
            var validationResult = _registerUserValidator.Validate(registerUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(new MyCustomHttpResponse(false, validationResult.Errors[0].ErrorMessage));
            }

            _unitOfWork.User.Add(new User
            {
                UserName = registerUser.UserName,
                UserEmail = registerUser.UserEmail,
                UserPassword = registerUser.UserPassword,
                ProofId = registerUser.ProofId
            });

            _unitOfWork.Save();
            return Ok(new { status = "success", message = "User registered successfully" });
        }
    }
}