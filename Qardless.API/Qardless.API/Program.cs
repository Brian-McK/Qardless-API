using Microsoft.EntityFrameworkCore;
using Qardless.API.Services;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database context and connection
builder.Services.AddDbContext<QardlessAPIContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("QardlessAPIConnection")
));

builder.Services.AddControllers().AddNewtonsoftJson(s =>
{
    // This allows HTTP PATCH method
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

// Service Lifetime: Scoped, created once per request.
builder.Services.AddScoped<IQardlessAPIRepo, SqlQardlessAPIRepo>();

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
