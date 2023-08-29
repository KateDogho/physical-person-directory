using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhysicalPersonDirectory.Api.ActionFilters;
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

    [HttpPost]
    [ValidateParameters]
    [ProducesResponseType(typeof(CreatePhysicalPersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreatePhysicalPersonCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreatePhysicalPersonCommandResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(CreatePhysicalPersonCommandResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreatePhysicalPersonCommand command)
    {
        var result = await _mediator.Send(command);

        return Created("GetPhysicalPersonDetails", new { id = result.Id });
    }

    [HttpPut("{id:int}")]
    [ValidateParameters]
    [ProducesResponseType(typeof(UpdatePhysicalPersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UpdatePhysicalPersonCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UpdatePhysicalPersonCommandResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UpdatePhysicalPersonCommandResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePhysicalPersonCommand command)
    {
        command.Id = id;

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{id:int}/UploadImage")]
    [ProducesResponseType(typeof(UploadImageCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UploadImageCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UploadImageCommandResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UploadImageCommandResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImage(int id, IFormFile image)
    {
        var command = new UploadImageCommand
        {
            Id = id,
            Image = image
        };

        command.Id = id;
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PhysicalPersonsQueryResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(PhysicalPersonsQueryResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PhysicalPersonsQueryResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(PhysicalPersonsQueryResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] PhysicalPersonsQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetPhysicalPersonDetails")]
    [ProducesResponseType(typeof(PhysicalPersonDetailsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PhysicalPersonDetailsQueryResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PhysicalPersonDetailsQueryResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(PhysicalPersonDetailsQueryResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDetails(int id)
    {
        var result = await _mediator.Send(new PhysicalPersonDetailsQuery(id));

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(DeletePhysicalPersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DeletePhysicalPersonCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DeletePhysicalPersonCommandResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DeletePhysicalPersonCommandResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeletePhysicalPersonCommand(id));

        return Ok(result);
    }

    [HttpPost("{id}/AddRelatedPerson")]
    [ProducesResponseType(typeof(AddRelatedPersonCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AddRelatedPersonCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(AddRelatedPersonCommandResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AddRelatedPersonCommandResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddRelatedPerson(int id, [FromBody] AddRelatedPersonCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }


    [HttpGet("RelatedPersonsReport")]
    [ProducesResponseType(typeof(RelatedPhysicalPersonsReportQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RelatedPhysicalPersonsReportQueryResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RelatedPhysicalPersonsReportQueryResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(RelatedPhysicalPersonsReportQueryResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RelatedPersonsReport()
    {
        var result = await _mediator.Send(new RelatedPhysicalPersonsReportQuery());

        return Ok(result);
    }
}