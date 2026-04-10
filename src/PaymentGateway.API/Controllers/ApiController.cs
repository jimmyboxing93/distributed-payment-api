using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using SharedData.Interfaces;

namespace PaymentGateway.API.Controllers
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
			_logger.LogInformation("Get request received for Card Id: {CardId}", id);
			var user = _userInfo.GetCreditCard(id);

			if (user == null)
			{
				_logger.LogWarning("User Lookup failed: User {UserId} was unable to find a credit card associated with account", id);
				return NotFound($"User with Id: {id} was not found");
			}

			// Security check. Checks to see if the ID's match
			if (id != user.UserID)
			{
				return Unauthorized("You do not have permission to view this card");
			}

			return Ok(user);
		}

		// Writes data using post method
		[HttpPost]
		[Route("api/[controller]")]
		public IActionResult Payment(SharedData.Models.UserInfo model)
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
				return Ok(new { Message = "Payment processed successfully" });

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
			_logger.LogInformation("Edit request received for Card Id: {CardId}", id);
			var existing_user = _userInfo.GetCreditCard(id);

			if (existing_user == null)
			{
				return NotFound($"user with Id: {id} was not found");
			}

			// Security check
			if (id != existing_user.UserID)
			{
				_logger.LogWarning("Security Alert: Unauthorized edit attmept on ID: {Id}", id);
				return Unauthorized("You do not have permission to edit this record");
			}

			model.UserID = existing_user.UserID;
			_logger.LogWarning("Card ID: {CardId} was UPDATED.", id);
			_userInfo.EditCreditCard(model);
			return Ok();
		}
		// Deletes data using delete method
		[HttpDelete]
		[Route("api/[controller]/{id}")]
		public IActionResult DeleteCreditCard(Guid id)
		{
			_logger.LogInformation("Delete request received for Card Id: {CardId}", id);
			var user = _userInfo.GetCreditCard(id);
			if (user == null)
			{
				_logger.LogWarning("Delete failed: CardId: {CardID} was not found.", id);
				return NotFound($"User with Id: {id} was not found");
			}

			// Checking to see if Id matches user
			if (id != user.UserID)
			{
				_logger.LogWarning("Security Alert: Unauthorized delete attmept on ID: {Id}", id);
				return Unauthorized("You do not have permission to delete this card.");
			}
			_userInfo.DeleteCreditCard(user);
			_logger.LogWarning("Card ID: {CardId} was DELETED.", id);
			return NoContent();
		}
	}
}