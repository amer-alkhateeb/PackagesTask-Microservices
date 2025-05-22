using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackagesService.Application.Dtos;
using PackagesService.Application.Packages.Commands;
using PackagesService.Application.Packages.Queries;
using PackagesService.Domain.ValueObjects;

namespace PackagesService.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private ISender _mediator;
        public PackagesController(ISender mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePackageRequest request)
        {
            var command = request.Adapt<CreatePackageCommand>();


            var result  = await _mediator.Send(command);

            var response = result.Adapt<CreatePackageCommandResponse>();

            return CreatedAtAction("Create", new { id = response.Id }, response);
        }


        [HttpGet("{id}")]
        public async Task <IActionResult> GetById([FromQuery] Guid id)
        {
            var query = new GetPackageByIdQuery(PackageId.From(id));
            var result = await _mediator.Send(query);

            if (result is null)
                return NotFound();
            var response = result.Adapt<GetPackageByIdQueryResult>();
            return Ok(response);

        }
    }
}
