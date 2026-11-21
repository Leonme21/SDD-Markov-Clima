using Markov.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<PredictWeather>();
var app = builder.Build();
app.MapControllers();
app.Run();
