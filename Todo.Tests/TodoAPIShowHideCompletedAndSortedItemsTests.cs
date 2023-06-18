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
        [TestMethod]
        public async Task List_Should_Return_AllItems_When_VisibilityIsShowAll()
        {
            // Arrange
            var mockSender = new Mock<ISender>();
            var expectedItems = UseCommonForTests.GetTestTodoItems();
            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Ascending, ItemsVisibility.Show_All) as OkObjectResult;
            var resultList = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedItems.Count(), resultList.Count());
        }

        [TestMethod]
        public async Task List_Should_Return_CompletedItems_When_VisibilityIsShowCompleted()
        {
            // Arrange
            var mockSender = new Mock<ISender>();
            var allItems = UseCommonForTests.GetTestTodoItems();
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
            var mockSender = new Mock<ISender>();
            var allItems = UseCommonForTests.GetTestTodoItems();
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
            var mockSender = new Mock<ISender>();
            var expectedItems = UseCommonForTests.GetTestTodoItems();
            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Ascending, ItemsVisibility.Show_All) as OkObjectResult;
            var resultList = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultList);

            var sortedItems = expectedItems.OrderBy(x => x.Created);
            CollectionAssert.AreEqual(sortedItems.ToList(), resultList.OrderBy(x => x.Created).ToList());
        }

        [TestMethod]
        public async Task List_Should_Return_Items_SortedInDescendingOrder()
        {
            // Arrange
            var mockSender = new Mock<ISender>();
            var expectedItems = UseCommonForTests.GetTestTodoItems();
            mockSender
                .Setup(sender => sender.Send(It.IsAny<ListTodoItemsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedItems);

            var controller = new TodoController(mockSender.Object);

            // Act
            var result = await controller.List(DataSort.Descending, ItemsVisibility.Show_All) as OkObjectResult;
            var resultList = result.Value as IEnumerable<TodoItem>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(resultList);

            var sortedItems = expectedItems.OrderByDescending(x => x.Created);
            CollectionAssert.AreEqual(sortedItems.ToList(), resultList.OrderByDescending(x => x.Created).ToList());
        }



    }
}
