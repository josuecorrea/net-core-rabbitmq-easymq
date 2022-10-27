using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Serilog;
using System.Reflection;
using TesteRabbitMQ;

var builder = WebApplication.CreateBuilder(args);

SerilogExtension.AddSerilogApi(builder.Configuration);
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBrokerService, BrokerService>();
builder.Services.AddSingleton<IWorker, Worker>();

builder.Services.AddHostedService<BrokerServiceHandler>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestSerilLogMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
