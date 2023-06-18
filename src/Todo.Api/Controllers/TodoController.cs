using MediatR;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> List(DataSort sort, ItemsVisibility items)
    {
        try
        {
            var response = await _sender.Send(new ListTodoItemsRequest(sort, items));

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromBody] CreateTodoItemRequest request)
    {
        try
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
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPost("markCompleted")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkCompleted([FromBody] UpdateTodoItemRequest request)
    {
        try
        {
            if (request == null)
                return BadRequest();


            if (string.IsNullOrWhiteSpace(request.TodoId))
                return BadRequest("Item Id is Empty");

            var response = await _sender.Send(request);

            if (!response)
                return NotFound("Item Id is Not Found");

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

}