using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhysicalPersonDirectory.Application.Commands;

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
}