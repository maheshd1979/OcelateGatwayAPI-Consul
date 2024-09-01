using AuthService;
using Consul;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.RegisterServices(configuration);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    cfg.Address = new Uri("http://localhost:8500");
}));

builder.Services.AddSingleton<IHostedService, ConsulService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseHealthChecks("/health");

app.UseEndpointDefinitions();
app.MapHealthChecks("/health");

app.Run();


