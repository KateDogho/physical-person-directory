using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhysicalPersonDirectory.Application.Commands;
using PhysicalPersonDirectory.Application.Queries;

namespace PhysicalPersonDirectory.Api.Controllers.V1;

[Route("v1/[controller]")]
[ApiController]
public class PhysicalPersonsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PhysicalPersonsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost()]
    [ProducesResponseType(typeof(CreatePhysicalPersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreatePhysicalPersonCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreatePhysicalPersonCommandResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(CreatePhysicalPersonCommandResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreatePhysicalPersonCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    
    [HttpPost("UploadFile")]
    [ProducesResponseType(typeof(UploadFileCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UploadFileCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UploadFileCommandResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UploadFileCommandResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadFile([FromForm]UploadFileCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    
    [HttpGet()]
    [ProducesResponseType(typeof(PhysicalPersonsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PhysicalPersonsQueryResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PhysicalPersonsQueryResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(PhysicalPersonsQueryResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery]PhysicalPersonsQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PhysicalPersonDetailsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PhysicalPersonDetailsQueryResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PhysicalPersonDetailsQueryResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(PhysicalPersonDetailsQueryResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDetails(int id)
    {
        var result = await _mediator.Send(new PhysicalPersonDetailsQuery(id));

        return Ok(result);
    }
}