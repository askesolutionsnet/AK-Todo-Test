using Microsoft.AspNetCore.Mvc;
using Todo.Api.Handlers;

namespace Todo.Api.Controllers;

[ApiController]
[Route("todo")]
public class TodoController : ControllerBase
{
    private readonly ISender _sender;

    public TodoController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(DataSort sort, ItemsVisibility items) => Ok(await _sender.Send(new ListTodoItemsRequest(sort, items)));


    [HttpPost("create")]
    public async Task<IActionResult> Get([FromBody] CreateTodoItemRequest request)
    {
        if (request == null)
            return BadRequest();

        if (string.IsNullOrWhiteSpace(request.Text))
            return BadRequest("Text is Empty");

        var response = await _sender.Send(request);
        if (response.Equals(Guid.Empty))
        {
            return BadRequest();
        }
        return Ok(response);
    }


    [HttpPost("markCompleted")]
    public async Task<IActionResult> MarkCompleted([FromBody] UpdateTodoItemRequest request)
    { 
        var response = await _sender.Send(request);

        if (!response)
            return BadRequest();

        return Ok(response);
    }
}