using ManageMovies.Endpoints;
using ManageMovies.Persistence;
using ManageMovies.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MovieDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});
builder.Services.AddTransient<IMovieService, MovieService>();

var app = builder.Build();

// Ensure DB is created before anything else
await using (var serviceScope = app.Services.CreateAsyncScope())
{
    await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<MovieDbContext>())
    {
        await dbContext.Database.EnsureCreatedAsync();
    }
}

// Configure the HTTP request pipeline and middlewares
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapMovieEndpoints();




app.Run();

