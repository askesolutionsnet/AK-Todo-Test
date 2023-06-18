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
    public class TodoAPIMarkCompletedTests
    {
        private Mock<ISender> mockSender;
        private TodoController controller;
        private Guid itemId;

        [TestInitialize]
        public void Setup()
        {
            mockSender = new Mock<ISender>();
            controller = new TodoController(mockSender.Object);
            itemId = Guid.NewGuid();
        }

        [TestMethod]
        public async Task MarkCompleted_Should_Return_OkResult_When_ItemFound_And_SuccessfullyUpdated()
        {
            // Arrange
            mockSender
                .Setup(sender => sender.Send(It.IsAny<UpdateTodoItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.MarkCompleted(new UpdateTodoItemRequest(itemId.ToString())) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task MarkCompleted_Should_Return_NotFound_When_ItemNotFound()
        {
            // Arrange
            mockSender
                .Setup(sender => sender.Send(It.IsAny<UpdateTodoItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.MarkCompleted(new UpdateTodoItemRequest(itemId.ToString())) as NotFoundObjectResult;
            var NotFoundResult = (NotFoundObjectResult)result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Item Id is Not Found", NotFoundResult.Value);
        }

        [TestMethod]
        public async Task Markcompleted_Should_Return_BadRequest_When_Text_Is_Empty()
        {
            // Arrange


            var request = new UpdateTodoItemRequest("")
            {
                TodoId = ""
            };

            // Act
            var result = await controller.MarkCompleted(request);
            var badRequestResult = (BadRequestObjectResult)result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Item Id is Empty", badRequestResult.Value);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task Markcompleted_Should_Return_BadRequest_When_Request_Object_IsNull()
        {
            // Arrange

            // Act
            var result = await controller.MarkCompleted(null);
            var badRequestResult = (BadRequestResult)result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task MarkCompleted_Should_Return_InternalServerError_When_Exception_Is_Caught()
        {
            // Arrange
            var request = new UpdateTodoItemRequest("todoId");

            mockSender
                .Setup(sender => sender.Send(It.IsAny<UpdateTodoItemRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await controller.MarkCompleted(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));

            var statusCodeResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

    }
}
