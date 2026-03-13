using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCProject1.Controllers;
using MVCProject1.UserData;
using MVCProject1.Models;

namespace PaymentProcessing.Tests
{
	public class ApiControllerTests
	{
		// Testing null credit cards via post method. 
		[Fact]
		public void Payment_ReturnBadRequest_WhenCreditCardIsEmpty()
		{
			var mockService = new Mock<IUserInfo>();
			var mockLogger  = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockService.Object, mockLogger.Object);

			// Testing empty data
			var invalidModel = new UserInfo { creditCardNumber = "" };

			var result = controller.Payment(invalidModel);

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

			Assert.Equal("Credit card number is required", badRequestResult.Value);

			// Used to make sure the DB is never called. 
			mockService.Verify(s => s.AddCreditCard(It.IsAny<UserInfo>()), Times.Never);

		}
		// Testing happy path for post method
		[Fact]
		public void Payment_ReturnsOk_WhenDataIsValid() 
		{
			var mockService = new Mock<IUserInfo>();
			var mockLogger = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockService.Object, mockLogger.Object);

			// Guard clause
			var validModel = new UserInfo
			{
				UserID = Guid.NewGuid(),
				creditCardNumber = "1234567890"
			};

			var result = controller.Payment(validModel);

			Assert.IsType<OkObjectResult>(result);

			mockService.Verify(s => s.AddCreditCard(It.IsAny<UserInfo>()), Times.Once);

		}
	}
}
