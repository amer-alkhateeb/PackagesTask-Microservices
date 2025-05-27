using DeliveryService.API.Contracts.DTOs;
using DeliveryService.Application.DeliveryRoutes.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{trackingCode}")]
        public async Task<ActionResult<DeliveryResponse>> GetByTrackingCode(string trackingCode)
        {
            var delivery = await _mediator.Send(new GetDeliveryByTrackingCodeQuery(trackingCode));
            if (delivery == null)
                return NotFound();
            return Ok(delivery.DeliveryDetails.Adapt<DeliveryResponse>());
        }
    }
}
