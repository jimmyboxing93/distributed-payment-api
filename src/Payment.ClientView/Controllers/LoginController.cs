using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.ClientView.Services;
using SharedData.Interfaces;
using ViewApi.Models;




namespace PaymentGateway.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LoginController : Controller
    {
        private readonly UserManager<SharedData.Models.User> _userManager;
        private readonly SignInManager<SharedData.Models.User> _signInManager;
        private readonly ITokenService _tokenService;
		private readonly IUserInfo _userInfo;

		public LoginController(UserManager<SharedData.Models.User> userManager, SignInManager<SharedData.Models.User> signInManager, ITokenService tokenService, IUserInfo userInfo)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _tokenService = tokenService;
			_userInfo = userInfo;
        }


		// GET: Login
		[HttpGet("Login")]
		public IActionResult Login()
        {
            return View();
        }

		[HttpGet("Register")]
		public IActionResult Register()
        {
            return View();
        }

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {

			if (!ModelState.IsValid) return BadRequest(ModelState);

			var user = new SharedData.Models.User
			{
				UserName = model.Email,
				Email = model.Email
				
			};


			var result = await _userManager.CreateAsync(user, model.Password);


			if (result.Succeeded)
			{
				var initialProfile = new SharedData.Models.UserInfo
				{
					UserID = user.Id,
					FirstName = string.Empty,
					LastName = string.Empty,
					creditCardNumber = string.Empty,
					Name = model.Email,
					LastFourDigits = string.Empty
				};
				try
				{

					_userInfo.AddCreditCard(initialProfile);

					return Ok(new { status = "Success", message = "User registered and profile created successfully!" });
				}
				catch (Exception)
				{
					// 5. Production Compensating Action: Rollback identity if data seeding fails
					await _userManager.DeleteAsync(user);
					return StatusCode(500, "Critical failure initializing application profile context. Account rolled back.");
				}
			}

			return BadRequest(result.Errors);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

			if (!ModelState.IsValid) return BadRequest(ModelState);

			
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = _tokenService.CreateToken(user);
                return Ok(new { token });
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            
			return Unauthorized(new { status = "Error", message = "Invalid Login Attempt" });
		}

	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public ActionResult ConfirmCode()
        {
            return View();
        }

		[HttpGet("/test-token")]
		public async Task<IActionResult> TestToken()
		{

            var realUser = await _userManager.Users.FirstOrDefaultAsync();

            if (realUser == null) 
            {
                return NotFound(new
                {
                    Status = "Error",
                    message = "No users found in the DB"
                });
            }

			// generates a token
			var token = _tokenService.CreateToken(realUser);

			return Ok(new
			{
				status = "Success",
				message = $"JWT generated for user: {realUser.Email}",
				token = token
			});
		}
	}
}