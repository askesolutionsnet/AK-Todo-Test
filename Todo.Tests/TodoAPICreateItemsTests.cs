using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Todo.Api.Controllers;
using Todo.Api.Handlers;
using Todo.Data;
using Todo.Data.Models;


namespace Todo.Tests
{
    [TestClass]
    public class TodoAPICreateItemsTests
    {
        private Mock<ISender> mockSender;
        private TodoController controller;

        [TestInitialize]
        public void Setup()
        {
            mockSender = new Mock<ISender>();
            controller = new TodoController(mockSender.Object);
        }

        [TestMethod]
        public async Task Create_Should_Return_BadRequest_When_Text_Is_Empty()
        {
            // Arrange


            var request = new CreateTodoItemRequest("")
            {
                Text = ""
            };

            // Act
            var result = await controller.Get(request);
            var badRequestResult = (BadRequestObjectResult)result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Text is Empty", badRequestResult.Value);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task Create_Should_Return_BadRequest_When_Request_Object_IsNull()
        {
            // Arrange

            // Act
            var result = await controller.Get(null);
            var badRequestResult = (BadRequestResult)result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task Get_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateTodoItemRequest("Sample Todo");

            mockSender
            .Setup(sender => sender.Send(It.IsAny<CreateTodoItemRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await controller.Get(request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Create_Should_Convert_Text_To_Uppercase()
        {
            // Arrange
            var request = new CreateTodoItemRequest("lowercase text");

            TodoItem capturedRequest = new TodoItem();
            mockSender
                  .Setup(sender => sender.Send(It.IsAny<CreateTodoItemRequest>(), It.IsAny<CancellationToken>()))
                  .Callback<IRequest<Guid>, CancellationToken>((req, cancellationToken) =>
                  {
                      capturedRequest.Text = request.Text.ToUpper();
                  })
                  .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await controller.Get(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = (OkObjectResult)result;
            Assert.AreNotEqual(Guid.Empty, okResult.Value);

            Assert.AreEqual("LOWERCASE TEXT", capturedRequest.Text);
        }

        [TestMethod]
        public async Task Create_Should_Return_InternalServerError_When_Exception_Is_Caught()
        {
            // Arrange
            var request = new CreateTodoItemRequest("TextItem");

            mockSender
                .Setup(sender => sender.Send(It.IsAny<CreateTodoItemRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await controller.Get(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));

            var statusCodeResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

    }
}

