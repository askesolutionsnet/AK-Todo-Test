using MediatR;
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
        [TestMethod]
        public async Task MarkCompleted_Should_Return_OkResult_When_ItemFound_And_SuccessfullyUpdated()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var mockSender = new Mock<ISender>();
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
        public async Task MarkCompleted_Should_Return_BadRequest_When_ItemNotFound()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var mockSender = new Mock<ISender>();
            mockSender
                .Setup(sender => sender.Send(It.IsAny<UpdateTodoItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.MarkCompleted(new UpdateTodoItemRequest(itemId.ToString())) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

    }
}
