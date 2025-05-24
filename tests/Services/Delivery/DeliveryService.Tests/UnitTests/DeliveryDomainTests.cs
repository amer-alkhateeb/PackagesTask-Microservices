using DeliveryService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Tests.UnitTests
{
    public class DeliveryDomainTests
    {
        [Fact]
    public void CreateDelivery_WithValidData_ShouldSucceed()
    {
        // Arrange
        string packageId = Guid.NewGuid().ToString();
        DateTime estimatedTime = DateTime.Now.AddDays(4);

        // Act
        var delivery = Delivery.Schedule(packageId, estimatedTime);

        // Assert
        Assert.NotNull(delivery);
        Assert.NotNull(delivery.Id);
        Assert.Equal(packageId, delivery.PackageId);
        Assert.Equal(estimatedTime, delivery.EstimatedTime);

    }


}
}
