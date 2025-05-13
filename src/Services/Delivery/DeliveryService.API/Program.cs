using DeliveryService.API.Mapping;
using DeliveryService.API.Middleware;
using DeliveryService.Application;
using DeliveryService.Infrastructure;
using DeliveryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
MappingConfig.RegisterMappings();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
    dbContext.Database.Migrate();
}


app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Packages API V1");
    options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();