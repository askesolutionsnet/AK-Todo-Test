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
    public class TodoAPIShowHideCompletedAndSortedItemsTests
    {
        private Mock<ISender> mockSender;
        private TodoController controller;
        private IEnumerable<TodoItem> allItems;

        [TestInitialize]
        public void Setup()
        {
            mockSender = new Mock<ISender>();
            controller = new TodoController(mockSender.Object);
            allItems = UseCommonForTests.GetTestTodoItems();
        }

        [TestMethod]
        public async Task List_Should_Return_AllItems_When_VisibilityIsShowAll()
        {
            // Arrange
              mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(allItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Ascending, ItemsVisibility.Show_All) as OkObjectResult;
            var resultList = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultList);
            Assert.AreEqual(allItems.Count(), resultList.Count());
        }

        [TestMethod]
        public async Task List_Should_Return_CompletedItems_When_VisibilityIsShowCompleted()
        {
            // Arrange
            var expectedItems = allItems.Where(x => x.Completed.HasValue).ToList();
            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(allItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Ascending, ItemsVisibility.Show_Completed) as OkObjectResult;
            var resultList = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedItems.Count(), resultList.Where(x=>x.Completed.HasValue).Count());
            CollectionAssert.AreEquivalent(expectedItems, resultList.Where(x => x.Completed.HasValue).ToList());
        }

        [TestMethod]
        public async Task List_Should_Return_NonCompletedItems_When_VisibilityIsHideCompleted()
        {
            // Arrange
            var expectedItems = allItems.Where(x => !x.Completed.HasValue).ToList();
            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(allItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Ascending, ItemsVisibility.Hide_Completed) as OkObjectResult;
            var resultList = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedItems.Count(), resultList.Where(x => !x.Completed.HasValue).Count());
            CollectionAssert.AreEquivalent(expectedItems, resultList.Where(x => !x.Completed.HasValue).ToList());
        }

        [TestMethod]
        public async Task List_Should_Return_Items_SortedInAscendingOrder()
        {
            // Arrange
            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(allItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Ascending, ItemsVisibility.Show_All) as OkObjectResult;
            var resultList = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultList);

            var sortedItems = allItems.OrderBy(x => x.Created);
            CollectionAssert.AreEqual(sortedItems.ToList(), resultList.OrderBy(x => x.Created).ToList());
        }

        [TestMethod]
        public async Task List_Should_Return_Items_SortedInDescendingOrder()
        {
            // Arrange
            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(allItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Descending, ItemsVisibility.Show_All) as OkObjectResult;
            var resultList = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultList);

            var sortedItems = allItems.OrderByDescending(x => x.Created);
            CollectionAssert.AreEqual(sortedItems.ToList(), resultList.OrderByDescending(x => x.Created).ToList());
        }



    }
}
