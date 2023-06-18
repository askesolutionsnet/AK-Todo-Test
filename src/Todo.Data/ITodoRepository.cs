using Todo.Data.Models;

namespace Todo.Data;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> List();
    
    Task<Guid> Create(TodoItem newItem);
}