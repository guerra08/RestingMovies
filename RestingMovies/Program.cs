using Microsoft.EntityFrameworkCore;
using RestingMovies.Api.Endpoints;
using RestingMovies.Api.Persistence;
using RestingMovies.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MoviesDbContext>(c => c.UseInMemoryDatabase("RestingMovies"));
builder.Services.AddDbContext<RatingsDbContext>(c => c.UseInMemoryDatabase("RestingMovies"));
builder.Services.AddTransient<IMovieRepository, MovieRepository>();
builder.Services.AddTransient<IRatingRepository, RatingRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMovieEndpoints();
app.MapRatingEndpoints();

app.Run();