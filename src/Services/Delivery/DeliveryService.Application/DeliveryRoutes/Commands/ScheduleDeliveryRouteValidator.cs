using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Application.DeliveryRoutes.Commands
{
    public sealed class ScheduleDeliveryRouteValidator : AbstractValidator<ScheduleDeliveryRouteCommand>
    {
        public ScheduleDeliveryRouteValidator()
        {
            RuleFor(x => x.TruckId).NotEmpty();
            RuleFor(x => x.DriverId).NotEmpty();
            RuleFor(x => x.ScheduledDate).GreaterThan(DateTime.UtcNow);
            RuleFor(x => x.PackageIds).NotEmpty();
        }
    }
}
