using ApiGateway;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Ocelot.Provider.Consul;
using Consul;

var builder = WebApplication.CreateBuilder(args);

// Configure HttpClient to ignore SSL certificate errors
//builder.Services.AddHttpClient("Ocelot")
//    .ConfigurePrimaryHttpMessageHandler(() =>
//    {
//        var handler = new HttpClientHandler();
//        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
//        return handler;
//    });

var configuration = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("ocelot.json");

builder.Services.AddOcelot()
    .AddConsul()
    .AddPolly();

builder.Services.AddSingleton<IConsulClient>(sp => new ConsulClient(cfg =>
{
    cfg.Address = new Uri("http://localhost:8500");
}));

builder.Services.RegisterServices(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOcelot().Wait();
app.Run();

