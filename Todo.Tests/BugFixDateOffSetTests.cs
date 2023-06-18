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

        private Mock<ISender> mockSender;
        private TodoController controller;

        [TestInitialize]
        public void Setup()
        {
            mockSender = new Mock<ISender>();
            controller = new TodoController(mockSender.Object);
        }


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
        public async Task API_List_Should_Return_With_Correct_Created_Field_DateTimeOffSet_Success()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Created = DateTime.Now.ToUniversalTime(), Text = "Item 1", Completed = null },
                new TodoItem { Id = Guid.NewGuid(), Created = DateTime.Now.ToUniversalTime(), Text = "Item 2", Completed = null }
            };

            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Ascending, ItemsVisibility.Show_All) as OkObjectResult;
            var items = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(items);
            Assert.AreEqual(todoItems.Count, items.Count());

            // Assert Created field correctness
            foreach (var item in items)
            {
                Assert.AreEqual(DateTimeKind.Utc, item.Created.Kind);
            }
        }

        [TestMethod]
        public async Task API_List_Should_Return_With_InCorrect_Created_Field_DateTimeOffSet_Fail()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Created = DateTime.Now, Text = "Item 1", Completed = null },
                new TodoItem { Id = Guid.NewGuid(), Created = DateTime.Now, Text = "Item 2", Completed = null }
            };

            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Ascending, ItemsVisibility.Show_All) as OkObjectResult;
            var items = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(items);
            Assert.AreEqual(todoItems.Count, items.Count());

            // Assert Created field correctness
            foreach (var item in items)
            {
                Assert.AreNotEqual(DateTimeKind.Utc, item.Created.Kind);
            }
        }

    }
}