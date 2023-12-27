using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient",
        builder => builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddDbContext<StoreContext>(o =>
    o.UseMySQL(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
