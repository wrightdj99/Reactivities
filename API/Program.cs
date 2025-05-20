using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//We're creating a new Database Context here, and using our Persistance.DbContext to do it. From here, we can pass options like our DefaultConnection connection string.
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:3000", "https://localhost:3000"));

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

/*
    With this following try-catch, we execute any 
    pending migrations and seed the database with data
    using the static SeedData method in case no data
    exists.
*/
try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedData(context);
}
catch (Exception e)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "An error occurred during migration.");
}

app.Run();
