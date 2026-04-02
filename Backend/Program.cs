using Backend.Data.Models;
using Backend.Data.Repositories;
using Backend.Features.Hello;
using Backend.Features.Status;
using Backend.Features.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddSingleton<IRepository<TaskItem>, InMemoryRepository<TaskItem>>();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Feature endpoints will be mapped here
app.MapHelloEndpoint();
app.MapStatusEndpoint();
app.MapTasksEndpoint();

app.Run();
