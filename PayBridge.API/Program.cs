using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PayPridge.Application.Interfaces;
using PayPridge.Application.Mapping;
using PayPridge.Application.Services;
using PayPridge.Domain.Interfaces;
using PayPridge.Infrastructure.BackgroundServices;
using PayPridge.Infrastructure.Data;
using PayPridge.Infrastructure.Messaging;
using PayPridge.Infrastructure.Repositories;
using RestSharp;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddHttpClient();
builder.Services.AddSingleton<RestClient>();

var sqlServerHost = Environment.GetEnvironmentVariable("SQLSERVER_HOST");
var sqlServerPort = Environment.GetEnvironmentVariable("SQLSERVER_PORT");
var sqlServerUser = Environment.GetEnvironmentVariable("SQLSERVER_USER");
var sqlServerPass = Environment.GetEnvironmentVariable("SQLSERVER_PASS");

var connectionString = $"Server={sqlServerHost},{sqlServerPort};Database=PayBridgeDb;User Id={sqlServerUser};Password={sqlServerPass};TrustServerCertificate=True;";

builder.Services.AddDbContext<PayBridgeContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PayPridge API", Version = "v1" });
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PreOrderConsumer>();
    x.AddConsumer<ProductConsumer>();
    x.AddConsumer<CompleteOrderConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(Environment.GetEnvironmentVariable("RABBITMQ_HOST"), h =>
        {
            h.Username(Environment.GetEnvironmentVariable("RABBITMQ_USER"));
            h.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASS"));
        });

        cfg.ReceiveEndpoint("pre-order-queue", e =>
        {
            e.ConfigureConsumer<PreOrderConsumer>(context);
        });

        cfg.ReceiveEndpoint("completed-order-queue", e =>
        {
            e.ConfigureConsumer<CompleteOrderConsumer>(context);
        });

        cfg.ReceiveEndpoint("product_queue", e =>
        {
            e.ConfigureConsumer<ProductConsumer>(context);
        });
    });
});


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IOrderPaymentService, PreOrderPaymentService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductProducerService, ProductProducerService>();

// Background Service
builder.Services.AddHostedService<ProductUpdateWorker>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PayBridgeContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PayPridge API v1");
    c.RoutePrefix = string.Empty;
});
//}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
