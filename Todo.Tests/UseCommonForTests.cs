using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Models;
using Todo.Data;

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
    }
}
