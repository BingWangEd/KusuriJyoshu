// using dotenv.net;
using kusuri.Classes;

var builder = WebApplication.CreateBuilder(args);

// DotEnv.Load();
// var googleApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<kusuri.Models.AppDbContext>();
builder.Services.AddControllers();
builder.Services.AddSingleton(serviceProvider => new RedisService("localhost:6379"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
