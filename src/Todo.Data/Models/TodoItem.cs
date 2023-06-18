namespace Todo.Data.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    
    public DateTime Created { get; set; }
    
    public string? Text { get; set; }
    
    public DateTime? Completed { get; set; }
}