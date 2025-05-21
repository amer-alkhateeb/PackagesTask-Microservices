﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public sealed record DeliveryScheduledIntegrationEvent(
        Guid RouteId,
        Guid DeliveryId,
        string PackageId,
        DateTime EstimatedTime
    );
}
