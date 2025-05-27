using DeliveryService.API.Contracts.DTOs;
using DeliveryService.Application.DeliveryRoutes.Commands;
using DeliveryService.Application.DeliveryRoutes.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoutesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRouteRequest request)
        {
            var command = request.Adapt<ScheduleDeliveryRouteCommand>();
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id.Id }, new { id.Id });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetRouteByIdQuery(id));
            if (result == null)
                return NotFound();
            var response = result.route.Adapt<RouteResponse>();
            return Ok(response);
        }
    }
}
