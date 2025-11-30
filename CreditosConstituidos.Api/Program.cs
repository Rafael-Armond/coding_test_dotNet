using CreditosConstituidos.Api.HostedService;
using CreditosConstituidos.Application.Interfaces;
using CreditosConstituidos.Application.UseCases;
using CreditosConstituidos.Infrastructure;
using CreditosConstituidos.Infrastructure.Messaging.Kafka;
using CreditosConstituidos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurando o PostgresSQL
builder.Services.AddDbContext<CreditoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<KafkaTopicInitializer>();

// Add services to the container.
builder.Services.AddScoped<ICreditoRepository, CreditoRepository>();
builder.Services.AddScoped<IntegrarCreditosConstituidosUseCase>();
builder.Services.AddScoped<ConsultarCreditosPorNfseUseCase>();
builder.Services.AddScoped<ConsultarCreditoPorNumeroUseCase>();
builder.Services.AddScoped<ProcessarCreditoRecebidoUseCase>();

// Kafka 
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<IMessageBusProducer, KafkaMessageBusProducer>();
builder.Services.AddSingleton<IMessageBusConsumer, KafkaMessageBusConsumer>();

// Hosted service
builder.Services.AddHostedService<CreditoBackgroundService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var initializer = scope.ServiceProvider.GetRequiredService<KafkaTopicInitializer>();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
