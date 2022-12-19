using CardApi.DBContext;
using CardApi.Models;
using CardApi.Interfaces;
using CardApi.Repositories;
using Microsoft.EntityFrameworkCore;
using CardApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adds a single instance of the Bingocard repository to the API.
builder.Services.AddTransient<IBingoCardRepository, BingoCardRepository>();

//Adds the database seed to the API
builder.Services.AddTransient<Seed>();

builder.Services.AddDbContext<CardContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("BingoConnStr")
        )
   );

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
    builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
    });
});

var app = builder.Build();

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    if (scopedFactory == null) return;

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        if (service != null) service.SeedContext();
    }
}

SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
