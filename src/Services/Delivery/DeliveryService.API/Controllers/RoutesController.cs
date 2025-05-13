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

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RouteResponse>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetRouteByIdQuery(id));
            var response = result.Adapt<RouteResponse>();
            return Ok(response);
        }
    }
}
