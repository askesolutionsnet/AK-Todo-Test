using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Models;
using Todo.Data;
using Todo.Api.Handlers;

namespace Todo.Tests
{
    public static class UseCommonForTests
    {
        public static DbContextOptions<TodoContext> GetInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "Todo")
                .Options;
        }

        public static TodoItem CreateSampleTodoItem()
        {
            return new TodoItem
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow, // Use UTC time to avoid timezone issues
                Text = "Sample todo item",
                Completed = null
            };
        }

        public static IEnumerable<TodoItem> GetTestTodoItems()
        {
            var items = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 1", Completed = null, Created = DateTime.UtcNow },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 2", Completed = DateTime.UtcNow, Created = DateTime.UtcNow.AddDays(-1) },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 3", Completed = DateTime.UtcNow, Created = DateTime.UtcNow.AddHours(-2) },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 4", Completed = null, Created = DateTime.UtcNow.AddDays(-3) },
                new TodoItem { Id = Guid.NewGuid(), Text = "Task 5", Completed = DateTime.UtcNow, Created = DateTime.UtcNow.AddMinutes(-30) }
            };

            return items;
        }

        private static bool MatchRequestTextToUpper(CreateTodoItemRequest request)
        {
            return request != null && request.Text == request.Text?.ToUpper();
        }
    }
}
