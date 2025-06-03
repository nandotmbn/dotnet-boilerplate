using Infrastructure.Services;
using GraphQL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Database Service with PostgreSQL sharding
builder.Services.DatabaseServices(builder.Configuration);
builder.Services.AuthenticationService(builder.Configuration);
builder.Services.QueryService(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL();

app.Run();
