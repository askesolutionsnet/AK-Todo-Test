using Todo.Data.Models;

namespace Todo.Api.Handlers;

public record ListTodoItemsRequest(DataSort Sort, ItemsVisibility Visibility) : IRequest<IEnumerable<TodoItem>>;

public enum ItemsVisibility
{
    Show_All = 1,
    Show_Completed = 2,
    Hide_Completed = 3
}

public enum DataSort
{
    Ascending = 1,
    Descending = 2
}

public class ListTodoItemsHandler : IRequestHandler<ListTodoItemsRequest, IEnumerable<TodoItem>>
{
    private readonly ITodoRepository _todoRepository;

    public ListTodoItemsHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IEnumerable<TodoItem>> Handle(ListTodoItemsRequest request, CancellationToken cancellationToken)
    {
        var data = await _todoRepository.List();
        switch (request.Visibility)
        {
            case ItemsVisibility.Show_Completed:
                data = data.Where(x => x.Completed.HasValue).ToList();
                break;
            case ItemsVisibility.Hide_Completed:
                data = data.Where(x => !x.Completed.HasValue).ToList();
                break;
            default:
                break;
        }

        switch (request.Sort)
        {
            case DataSort.Descending:
                data = data.OrderByDescending(x => x.Created).ToList();
                break;
            case DataSort.Ascending:
            default:
                data = data.OrderBy(x => x.Created).ToList();
                break;
        }

        return data;
    }

}