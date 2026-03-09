using Microsoft.AspNetCore.Mvc;
using MVCProject1.Models;
using MVCProject1.UserData;

namespace MVCProject1.Controllers
{
    // Creates API endpoint
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IUserInfo _userInfo;
        private readonly ILogger<ApiController> _logger;

        public ApiController(IUserInfo userInfo, ILogger<ApiController> logger)
        {
            _userInfo = userInfo;
            _logger = logger;
        }
        // Reads data using GET method
        [HttpGet]
        [Route("api/[controller]")]
        public ActionResult GetCreditCards()
        {
            
            return Ok(_userInfo.GetCreditCards());
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public ActionResult GetCreditCard(Guid id)
        {
            var user = _userInfo.GetCreditCard(id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound($"User with Id: {id} was not found");
        }

        // Writes data using post method
        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult Payment(UserInfo model)
        {
            _logger.LogInformation("Processing payment request for User: {userId}", model.UserID);
            // Guard clause
            if (string.IsNullOrEmpty(model.creditCardNumber)) 
            {
                _logger.LogWarning("Payment failed: User {UserId} submitted an empty credit card number.", model.UserID);
                return BadRequest("Credit card number is required");
            }
            try
            {
                _userInfo.AddCreditCard(model);
                _logger.LogInformation("Succesfully processed payment for user: {userID}", model.UserID);
                return Ok(new { Message = "Payment proccced succesfully" });

            }

            catch (Exception ex) 
            {
                _logger.LogError(ex, "An unexpected error occurred while adding card for User: {UserId}", model.UserID);
                return StatusCode(500, "Internal server error.");
            }

           
        }
        // Edits data using put method
        [HttpPut]
        [Route("api/[controller]/{id}")]
        public IActionResult EditCreditCard(Guid id, UserInfo model)
        {
            var existing_user = _userInfo.GetCreditCard(id);

            if (existing_user != null)
            {
                model.UserID = existing_user.UserID;
                _userInfo.EditCreditCard(model);
               
            }
            return Ok(model);
        }
        // Deletes data using delete method
        
        [HttpDelete]
        [Route("api/[controller]/{id}")]
        public IActionResult DeleteCreditCard(Guid id)
        {
            var user = _userInfo.GetCreditCard(id);

            if (user != null)
            {
                _userInfo.DeleteCreditCard(user);
                return Ok();
            }
            return NotFound($"User with Id: {id} was not found");

        }

        


    }
}
