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
    public class BugFixDateOffSetTests
    {


        [TestMethod]
        public async Task Create_Should_Set_Created_In_UTC()
        {
            // Arrange
            var options =   UseCommonForTests.GetInMemoryDatabaseOptions();
            using (var context = new TodoContext(options))
            {
                var repository = new TodoRepository(context);
                var newItem = UseCommonForTests.CreateSampleTodoItem();

                // Act
                var itemId = await repository.Create(newItem);

                // Assert
                Assert.AreEqual(newItem.Id, itemId);
                var savedItem = context.TodoItems.FirstOrDefault(item => item.Id == itemId);
                Assert.IsNotNull(savedItem);
                Assert.AreEqual(newItem.Created, savedItem.Created);
                Assert.AreEqual(newItem.Created.Kind, DateTimeKind.Utc); // Verify that the Created field is in UTC
            }
        }

        [TestMethod]
        public async Task List_Should_Return_Items_With_Created_In_UTC()
        {
            // Arrange
            var options = UseCommonForTests.GetInMemoryDatabaseOptions();
            using (var context = new TodoContext(options))
            {
                var repository = new TodoRepository(context);
                var newItem = UseCommonForTests.CreateSampleTodoItem();
                await repository.Create(newItem);

                // Act
                var items = await repository.List();

                // Assert
                Assert.IsNotNull(items);
                Assert.IsTrue(items.Any());
                foreach (var item in items)
                {
                    Assert.AreEqual(item.Created.Kind, DateTimeKind.Utc); // Verify that the Created field is in UTC
                }
            }
        }

        [TestMethod]
        public async Task API_List_Should_Return_OkResult_With_Correct_Created_Field()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Created = DateTime.UtcNow, Text = "Item 1", Completed = null },
                new TodoItem { Id = Guid.NewGuid(), Created = DateTime.UtcNow, Text = "Item 2", Completed = null }
            };

            var mockSender = new Mock<ISender>();
            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List() as OkObjectResult;
            var items = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(items);
            Assert.AreEqual(todoItems.Count, items.Count());

            // Assert Created field correctness
            foreach (var item in items)
            {
                Assert.AreEqual(DateTime.UtcNow.Date, item.Created.Date);
            }
        }

    }
}