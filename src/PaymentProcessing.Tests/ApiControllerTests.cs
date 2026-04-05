using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.UserData;
using PaymentGateway.API.Models;

namespace PaymentProcessing.Tests
{
	public class ApiControllerTests
	{
		// Happy path get method
		[Fact]
		public void GetCreditCard_Ok_WhenUserIsOwner() 
		{
			// Arrange, act, assert
			var mockService = new Mock<IUserInfo>();
			var mockLogger = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockService.Object, mockLogger.Object);

			var userId = Guid.NewGuid();

			var existingUser = new UserInfo
			{
				UserID = userId,
				creditCardNumber = "1234"
			};

			mockService.Setup(s => s.GetCreditCard(userId)).Returns(existingUser);

			var result = controller.GetCreditCard(userId);
			var okResult = Assert.IsType<OkObjectResult>(result);

			Assert.Equal(existingUser, okResult.Value);


		}

		// Get user not found path
		[Fact]
		public void GetCrediCard_ReturnNotFound_WhenUserIdDoesNotExist() 
		{
			// Setting up arrange, act, assert, and verify
			var mockService = new Mock<IUserInfo>();
			var mockLogger = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockService.Object, mockLogger.Object);

			// Retrun Null for any ID
			mockService.Setup(S => S.GetCreditCard(It.IsAny<Guid>())).Returns((UserInfo)null);

			var result = controller.GetCreditCard(Guid.NewGuid());

			Assert.IsType<NotFoundObjectResult>(result);
		}

		// Get method sad path 
		[Fact]
		public void GetCreditCard_ReturnsUnathorized_WhenUserIsNotOwner() 
		{
			// Setting up Arrange, act, assert and verify. 
			var mockServie = new Mock<IUserInfo>();
			var mockLogger = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockServie.Object, mockLogger.Object);

			var ownerId = Guid.NewGuid();
			var hackerId = Guid.NewGuid();

			var existingCard = new UserInfo
			{

				UserID = ownerId,
				creditCardNumber = "111"
			};

			mockServie.Setup(s => s.GetCreditCard(hackerId)).Returns(existingCard);

			var result = controller.GetCreditCard(hackerId);

			Assert.IsType<UnauthorizedObjectResult>(result);
		}

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

		// Security test
		[Fact]
		public void Edit_ReturnsUnathorized_WhenUserIsNotOwner() 
		{
			// Setup arrange, assert, verify logic
			var mockService = new Mock<IUserInfo>();
			var mockLogger = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockService.Object, mockLogger.Object);

			var existingCard = new UserInfo
			{
				UserID = Guid.NewGuid(),
				creditCardNumber = "1111"
			};

			var hackerAttempt = new UserInfo
			{
				UserID = Guid.NewGuid(),
				creditCardNumber = "2222"
			};

			mockService.Setup(s => s.GetCreditCard(It.IsAny<Guid>())).Returns(existingCard);

			var result = controller.EditCreditCard(hackerAttempt.UserID, hackerAttempt);

			Assert.IsType<UnauthorizedObjectResult>(result);

			mockService.Verify(s => s.EditCreditCard(It.IsAny<UserInfo>()), Times.Never);
		}

		// Edit happy path
		[Fact]
		public void Edit_ReturnsOk_WhenUserIsOwner() 
		{
			var mockService = new Mock<IUserInfo>();
			var mockLogger = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockService.Object, mockLogger.Object);


			var userId = Guid.NewGuid();

			var existingCard = new UserInfo
			{
				UserID = userId,
				creditCardNumber = "1111"
			};

			var updatedInput = new UserInfo
			{
				UserID = userId,
				creditCardNumber = "2222"
			};

			mockService.Setup(s => s.GetCreditCard(userId)).Returns(existingCard);

			var result = controller.EditCreditCard(updatedInput.UserID, updatedInput);

			Assert.IsType<OkResult>(result);

			mockService.Verify(s => s.EditCreditCard(It.IsAny<UserInfo>()), Times.Once);


		}

		// Delete Happy path
		[Fact]
		public void Delete_ReturnsOk_WhenUserIsOwner() 
		{
			// Setup arrange, act, assert, verify logic
			var mockService = new Mock<IUserInfo>();
			var mockLogger = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockService.Object, mockLogger.Object);

			var userId = Guid.NewGuid();

			var existingCard = new UserInfo
			{
				UserID = userId,
				creditCardNumber = "1111"
			};

			mockService.Setup(s => s.GetCreditCard(userId)).Returns(existingCard);

			var result = controller.DeleteCreditCard(userId);

			Assert.IsType<NoContentResult>(result);

			mockService.Verify(s => s.DeleteCreditCard(It.IsAny<UserInfo>()), Times.Once);


		}

		// Delete sad path
		[Fact]
		public void Delete_ReturnsUnathorized_WhenUserIsNotOwner() 
		{
			//Setup arrgange, act, assert, verify
			var mockService = new Mock<IUserInfo>();
			var mockLogger = new Mock<ILogger<ApiController>>();
			var controller = new ApiController(mockService.Object, mockLogger.Object);


			var existingCard = new UserInfo
			{
				UserID = Guid.NewGuid(),
				creditCardNumber = "1111"
			};

			var hackerAttempt = new UserInfo
			{
				UserID = Guid.NewGuid(),
				creditCardNumber = "2222"
			};

			mockService.Setup(s => s.GetCreditCard(hackerAttempt.UserID)).Returns(existingCard);

			var result = controller.DeleteCreditCard(hackerAttempt.UserID);

			Assert.IsType<UnauthorizedObjectResult>(result);

			mockService.Verify(s => s.DeleteCreditCard(It.IsAny<UserInfo>()), Times.Never);
		}

	}
}
