using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApplication19.DbConnection;
using WebApplication19.MongoDbService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ?? Read config from appsettings.json
builder.Services.Configure<BusBookingDatabaseSettings>(
    builder.Configuration.GetSection("BusBookingDatabase"));



// ?? Register MongoClient as Singleton
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = s.GetRequiredService<IOptions<BusBookingDatabaseSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<BusService>();

var app = builder.Build();
// Use HTTPS redirection only when running locally (Development)
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
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
app.MapGet("/", () => "🚍 Bus Booking API is running on Render!");

app.Run();
