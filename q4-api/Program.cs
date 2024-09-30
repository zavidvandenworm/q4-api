using Application;
using dotenv.net;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using q4_api.Context;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Q4DbContext>(opts =>
{
    opts.UseMySql(Environment.GetEnvironmentVariable("CONNECTION_STRING"),
        new MySqlServerVersion(new Version(8, 0)));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();


// no cors
app.UseCors(
    options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
);


app.UseAuthorization();

app.MapControllers();

app.Run();