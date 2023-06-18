using Todo.Data.Models;

namespace Todo.Api.Handlers;

public record UpdateTodoItemRequest(string TodoId) : IRequest<bool>;

public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemRequest, bool>
{
    private readonly ITodoRepository _todoRepository;

    public UpdateTodoItemHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<bool> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var ret = false;
        if (!string.IsNullOrEmpty(request.TodoId))
        {
            var items = await _todoRepository.List();
            if (items != null)
            {
                var itemToUpdate = items.Where(x => x.Id.ToString() == request.TodoId).FirstOrDefault();
                if (itemToUpdate != null)
                {
                    itemToUpdate.Completed = DateTime.Now.ToUniversalTime();
                    ret = await _todoRepository.UpdateAsync(itemToUpdate);
                }
            }
        }
        return ret;
    }
}