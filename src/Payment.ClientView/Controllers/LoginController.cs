using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.ClientView.Services;
using ViewApi.Models;

namespace PaymentGateway.API.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;

		public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _tokenService = tokenService;
        }


        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("login", "login");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var token = _tokenService.CreateToken(user);
                    return Ok(new { token });
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
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